﻿using System;
using System.Collections.Generic;

namespace CSharpGL
{
    internal class ClockMarkRenderer : RendererBase, IRenderable, IWorldSpace
    {
        private readonly List<vec3> markPosition = new List<vec3>();
        private readonly List<vec3> markColor = new List<vec3>();

        public ClockMarkRenderer()
        {
            this.Scale = new vec3(1, 1, 1);
            this.ModelSize = new vec3(2, 2, 2);
        }

        protected override void DoInitialize()
        {
            int markCount = 60;
            for (int i = 0; i < markCount; i++)
            {
                var position = new vec3(
                    (float)Math.Cos((double)i / (double)markCount * Math.PI * 2),
                    (float)Math.Sin((double)i / (double)markCount * Math.PI * 2),
                    0);
                markPosition.Add(position);
                markColor.Add(new vec3(1, 1, 1));

                var position2 = new vec3(
                    (float)Math.Cos((double)i / (double)markCount * Math.PI * 2),
                    (float)Math.Sin((double)i / (double)markCount * Math.PI * 2),
                    0) * (i % 5 == 0 ? 0.8f : 0.9f);
                markPosition.Add(position2);
                markColor.Add(new vec3(1, 1, 1));
            }
        }

        /// <summary>
        /// Render something.
        /// </summary>
        /// <param name="arg"></param>
        public void Render(RenderEventArgs arg)
        {
            if (!this.IsInitialized) { Initialize(); }

            DoRender(arg);
        }

        /// <summary>
        /// Render something.
        /// </summary>
        /// <param name="arg"></param>
        protected void DoRender(RenderEventArgs arg)
        {
            GL.Instance.MatrixMode(GL.GL_PROJECTION);
            GL.Instance.LoadIdentity();
            arg.Scene.Camera.LegacyProjection();
            GL.Instance.MatrixMode(GL.GL_MODELVIEW);
            GL.Instance.LoadIdentity();
            this.LegacyTransform();

            GL.Instance.Begin((uint)DrawMode.Lines);
            for (int i = 0; i < markPosition.Count; i++)
            {
                vec3 color = markColor[i];
                GL.Instance.Color3f(color.x, color.y, color.z);
                vec3 position = markPosition[i];
                GL.Instance.Vertex3f(position.x, position.y, position.z);
            }
            GL.Instance.End();
        }

        #region IWorldSpace 成员

        /// <summary>
        /// 
        /// </summary>
        public vec3 WorldPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float RotationAngle { get; set; }

        private vec3 _rotationAxis = new vec3(0, 1, 0);
        /// <summary>
        /// 
        /// </summary>
        public vec3 RotationAxis { get { return this._rotationAxis; } set { this._rotationAxis = value; } }

        private vec3 _scale = new vec3(1, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        public vec3 Scale { get { return this._scale; } set { this._scale = value; } }

        private vec3 _modelSize = new vec3(1, 1, 1);
        /// <summary>
        /// 
        /// </summary>
        public vec3 ModelSize { get { return this._modelSize; } set { this._modelSize = value; } }

        #endregion
    }
}