using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemEx;
using NAudio.Lame;

namespace ConvertAllToMp3
{
    public class Arguments
    {
        public static Arguments Parse(string[] args)
        {
            var exclude = new List<string>();
            var afterConvert = AfterConvert.Ask;
            var quality = LAMEPreset.V2;
            string directory = null;

            bool hadExclude = false;
            bool hadQuality = false;

            foreach (string arg in args)
            {
                if (hadExclude)
                {
                    exclude.Add(arg);
                    hadExclude = false;
                }
                else if (hadQuality)
                {
                    quality = Enum<LAMEPreset>.Parse(arg, true);
                    hadQuality = false;
                }
                else
                {
                    switch (arg)
                    {
                        case "--delete":
                            afterConvert = AfterConvert.Delete;
                            break;
                        case "--recycle":
                            afterConvert = AfterConvert.Recycle;
                            break;
                        case "--exclude":
                            hadExclude = true;
                            break;
                        case "--quality":
                            hadQuality = true;
                            break;
                        default:
                            directory = arg;
                            break;
                    }
                }
            }

            if (hadExclude)
                throw new Exception("Missing argument to --exclude");
            if (hadQuality)
                throw new Exception("Missing argument to --quality");
            if (directory == null)
                throw new Exception("Missing directory");

            return new Arguments(directory, afterConvert, quality, exclude);
        }

        public string Directory { get; }
        public AfterConvert AfterConvert { get; }
        public LAMEPreset Quality { get; }
        public IList<string> Exclude { get; }

        private Arguments(string directory, AfterConvert afterConvert, LAMEPreset quality, List<string> exclude)
        {
            Directory = directory;
            AfterConvert = afterConvert;
            Quality = quality;
            Exclude = new ReadOnlyCollection<string>(exclude);
        }
    }
}
