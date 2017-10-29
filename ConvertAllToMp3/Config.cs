using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertAllToMp3
{
    public class Config
    {
        private static readonly XNamespace Ns = "https://github.com/pvginkel/ConvertAllToMp3";

        public static Config Load(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var document = XDocument.Load(fileName);
            var excludes = new List<string>();

            foreach (XElement element in document.Root.Elements(Ns + "exclude"))
            {
                excludes.Add(element.Value);
            }

            return new Config(excludes);
        }

        private readonly List<string> _excludes;

        private Config(List<string> excludes)
        {
            _excludes = excludes;
        }

        public bool Include(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            foreach (string exclude in _excludes)
            {
                if (fileName.IndexOf(exclude, StringComparison.OrdinalIgnoreCase) != -1)
                    return false;
            }

            return true;
        }
    }
}
