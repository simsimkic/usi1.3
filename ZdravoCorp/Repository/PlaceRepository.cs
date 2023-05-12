using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Model;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Repository
{
    public class PlaceRepository
    {
        private readonly string _filePath = ResourcePath.DataPath + "places.csv";
        private readonly Serializer<Place> _serializer;
        private List<Place> places;

        public PlaceRepository()
        {
            _serializer = new Serializer<Place>();
            places = new List<Place>();
            Load();
        }

        public List<Place> Places { get { return places; } }

        public void Load()
        {
            places = _serializer.FromCSV(_filePath);
        }

    }
}
