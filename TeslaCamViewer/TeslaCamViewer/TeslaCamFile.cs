using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeslaCamViewer
{
    /// <summary>
    /// A single TeslaCam File
    /// </summary>
    public class TeslaCamFile
    {
        public enum CameraType
        {
            UNKNOWN,
            LEFT_REPEATER,
            FRONT,
            RIGHT_REPEATER
        }
        private static readonly Regex FileNameRegex = new Regex("^(?<date>[0-9]{4}-[0-9]{2}-[0-9]{2}_[0-9]{2}-[0-9]{2}-[0-9]{2})-(?<camera>[a-z_]*).mp4$");
        public string FilePath { get; private set; }
        public string FileName { get { return System.IO.Path.GetFileName(FilePath); } }
        public TeslaCamDate Date { get; private set; }
        public CameraType CameraLocation { get; private set; }
        public string FileDirectory { get { return System.IO.Path.GetDirectoryName(FilePath); } }
        public Uri FileURI { get { return new Uri(this.FilePath); } }

        private TeslaCamFile()
        {
        }

        public static TeslaCamFile TryParse(string filePath)
        {
            TeslaCamFile file = new TeslaCamFile
            {
                FilePath = filePath
            };

            var m = FileNameRegex.Match(file.FileName);
            if (!m.Success)
                return null;
            file.Date = new TeslaCamDate(m.Groups["date"].Value);
            string cameraType = m.Groups["camera"].Value;
            if (cameraType == "front")
                file.CameraLocation = CameraType.FRONT;
            else if (cameraType == "left_repeater")
                file.CameraLocation = CameraType.LEFT_REPEATER;
            else if (cameraType == "right_repeater")
                file.CameraLocation = CameraType.RIGHT_REPEATER;
            else
                return null;

            return file;
        }

    }
}
