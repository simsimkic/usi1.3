using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class Equipment : ISerializable
    {
        protected int id;
        protected int idPLace;
        public Equipment(int id, int idPLace)
        {
            this.id = id;
            this.idPLace = idPLace;
        }

        public int Id { get { return id; } set { id = value; } }
        public int IdPlace { get { return idPLace; } set { idPLace = value; } }

        public Equipment() { }

        public virtual void FromCSV(string[] values) { }

        public virtual string[] ToCSV(){ return null; }

    }
}
