namespace YulCustoms
{
    using System;

    using Newtonsoft.Json.Linq;

    public class CustomsDeclaration : IMessage
    {
        private readonly JObject jObject;

        public CustomsDeclaration():this(new JObject())
        {
            
        }

        public CustomsDeclaration(JObject jObject)
        {
            this.jObject = jObject;
        }

        public Guid Id {
            get
            {
                return   this.jObject.Value<Guid>("id");
            }
            set
            {
                this.jObject["id"] = value;
            }
        }

        public string Name
        {
            get
            {
                return this.jObject.Value<string>("name");
            }
            set
            {
                this.jObject["name"] = value;
            }
        }

        public string Citenzenship
        {
            get
            {
                return this.jObject.Value<string>("citenzenship");
            }
            set
            {
                this.jObject["citenzenship"] = value;
            }
        }

        public bool Resident
        {
            get
            {
                return this.jObject.Value<bool>("resident");
            }
            set
            {
                this.jObject["resident"] = value;
            }
        }

        public string Flight {
            get { return this.jObject.Value<string>("flight"); }
            set { jObject["flight"] = value; } 
        }
    }
}