using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZdravoCorp.Serializer
{
    public interface ISerializable
    {
        string[] ToCSV();
        void FromCSV(string[] values);
    }
}