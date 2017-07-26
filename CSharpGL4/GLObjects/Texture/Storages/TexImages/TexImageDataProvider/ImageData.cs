﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CSharpGL
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageData : LeveledData, IDisposable
    {
        private readonly Bitmap bitmap;
        private readonly int level;
        private readonly bool autoDispose;

        private System.Drawing.Imaging.BitmapData bmpData;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="level"></param>
        /// <param name="autoDispose"></param>
        public ImageData(Bitmap bitmap, int level, bool autoDispose)
        {
            this.bitmap = bitmap;
            this.level = level;
            this.autoDispose = autoDispose;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override void LockData(out int level, out IntPtr data)
        {
            level = this.level;
            this.bmpData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            data = bmpData.Scan0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void FreeData()
        {
            this.bitmap.UnlockBits(this.bmpData);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        } // end sub

        /// <summary>
        /// Destruct instance of the class.
        /// </summary>
        ~ImageData()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Backing field to track whether Dispose has been called.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Dispose managed and unmanaged resources of this instance.
        /// </summary>
        /// <param name="disposing">If disposing equals true, managed and unmanaged resources can be disposed. If disposing equals false, only unmanaged resources can be disposed. </param>
        private void Dispose(bool disposing)
        {
            if (this.disposedValue == false)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                } // end if

                // Dispose unmanaged resources.
                if (this.autoDispose)
                {
                    var bitmap = this.bitmap;
                    if (bitmap != null)
                    {
                        bitmap.Dispose();
                    }
                }
            } // end if

            this.disposedValue = true;
        } // end sub
    }
}
