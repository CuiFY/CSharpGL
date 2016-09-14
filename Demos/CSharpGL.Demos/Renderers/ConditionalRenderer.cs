﻿using System.Collections.Generic;
using System.Drawing;

namespace CSharpGL.Demos
{
    /// <summary>
    /// demostrates how to perform conditional rendering.
    /// </summary>
    internal class ConditionalRenderer : RendererBase
    {
        private const int xside = 5, yside = 5, zside = 5;
        private const int pointCount = 10000;
        private static readonly vec3 unitLengths = new vec3(1, 1, 1);
        private const float scaleFactor = 1.01f;

        private List<Tuple<RendererBase, RendererBase, Query>> coupleList = new List<Tuple<RendererBase, RendererBase, Query>>();
        private DepthMaskSwitch depthMaskSwitch = new DepthMaskSwitch(false);
        private ColorMaskSwitch colorMaskSwitch = new ColorMaskSwitch(false, false, false, false);

        private bool enableConditionalRendering = true;

        public bool ConditionalRendering
        {
            get { return enableConditionalRendering; }
            set { enableConditionalRendering = value; }
        }

        private bool renderBoundingBox = true;

        public bool RenderBoundingBox
        {
            get { return renderBoundingBox; }
            set { renderBoundingBox = value; }
        }

        private bool renderTargetModel = true;

        public bool RenderTargetModel
        {
            get { return renderTargetModel; }
            set { renderTargetModel = value; }
        }

        public static ConditionalRenderer Create()
        {
            var result = new ConditionalRenderer();

            for (int x = 0; x < xside; x++)
            {
                for (int y = 0; y < yside; y++)
                {
                    for (int z = 0; z < zside; z++)
                    {
                        var model = new RandomPointsModel(unitLengths, pointCount);
                        RandomPointsRenderer renderer = RandomPointsRenderer.Create(model);
                        renderer.PointColor = Color.FromArgb(
                            (int)((float)(x + 1) / (float)xside * 255),
                            (int)((float)(y + 1) / (float)yside * 255),
                            (int)((float)(z + 1) / (float)zside * 255));
                        //renderer.WorldPosition = new vec3(
                        //    (float)x / (float)(xside - 1) - 0.5f,
                        //    (float)y / (float)(yside - 1) - 0.5f,
                        //    (float)z / (float)(zside - 1) - 0.5f)
                        //    * new vec3(xside, yside, zside)
                        //    * unitLengths
                        //    * scaleFactor;// move a little longer.
                        renderer.WorldPosition =
                            (new vec3(x, y, z) * unitLengths * scaleFactor)
                            - (new vec3(xside - 1, yside - 1, zside - 1) * unitLengths * scaleFactor * 0.5f);
                        var cubeRenderer = CubeRenderer.Create(new Cube(unitLengths));
                        cubeRenderer.WorldPosition = renderer.WorldPosition;
                        var query = new Query();
                        result.coupleList.Add(new Tuple<RendererBase, RendererBase, Query>(cubeRenderer, renderer, query));
                    }
                }
            }

            result.Lengths = new vec3(xside + 1, yside + 1, zside + 1) * unitLengths * scaleFactor;

            return result;
        }

        private ConditionalRenderer()
        { }

        protected override void DoInitialize()
        {
            foreach (var item in this.coupleList)
            {
                item.Item1.Initialize();
                item.Item2.Initialize();
                item.Item3.Initialize();
            }
        }

        protected override void DoRender(RenderEventArgs arg)
        {
            if (this.ConditionalRendering)
            {
                this.depthMaskSwitch.On();
                this.colorMaskSwitch.On();
                foreach (var item in this.coupleList)
                {
                    item.Item3.BeginQuery(QueryTarget.AnySamplesPassed);
                    item.Item1.Render(arg);
                    item.Item3.EndQuery(QueryTarget.AnySamplesPassed);
                }
                this.colorMaskSwitch.Off();
                this.depthMaskSwitch.Off();
                foreach (var item in this.coupleList)
                {
                    item.Item3.BeginConditionalRender(ConditionalRenderMode.QueryByRegionWait);
                    if (this.renderBoundingBox) { item.Item1.Render(arg); }
                    if (this.renderTargetModel) { item.Item2.Render(arg); }
                    item.Item3.EndConditionalRender();
                }
            }
            else
            {
                foreach (var item in this.coupleList)
                {
                    if (this.renderBoundingBox) { item.Item1.Render(arg); }
                    if (this.renderTargetModel) { item.Item2.Render(arg); }
                }
            }
        }
    }
}