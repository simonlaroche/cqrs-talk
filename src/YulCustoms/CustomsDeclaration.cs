using System;
using Newtonsoft.Json.Linq;

namespace YulCustoms
{
    public class CustomsDeclaration : IMessage
    {
        private readonly JObject jObject;

        public CustomsDeclaration() : this(new JObject())
        {
        }

        public override string ToString()
        {
            return string.Format("Customs declaration for passenger {0} ({1}) {2} on flight {3}", Name, Citenzenship, Resident ? "R" : "V", Flight);
        }

        public CustomsDeclaration(JObject jObject)
        {
            this.jObject = jObject;
        }

        public string Name
        {
            get { return jObject.Value<string>("name"); }
            set { jObject["name"] = value; }
        }

        public string Citenzenship
        {
            get { return jObject.Value<string>("citenzenship"); }
            set { jObject["citenzenship"] = value; }
        }

        public bool Resident
        {
            get { return jObject.Value<bool>("resident"); }
            set { jObject["resident"] = value; }
        }

        public string Flight
        {
            get { return jObject.Value<string>("flight"); }
            set { jObject["flight"] = value; }
        }

        public Guid Id
        {
            get { return jObject.Value<Guid>("id"); }
            set { jObject["id"] = value; }
        }
    }
}