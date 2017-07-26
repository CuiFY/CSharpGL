﻿namespace CSharpGL
{
    internal abstract class ZeroIndexLineSearcher
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="lastVertexId"></param>
        /// <param name="picker"></param>
        /// <returns></returns>
        internal abstract uint[] Search(PickingEventArgs arg,
            uint lastVertexId, ZeroIndexPicker picker);
    }
}