// (c) 2011 http://madebits.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace LinqXPathEx
{
    public static class LinqXPathExtession
    {
        public static object XPathEvaluateEx(this XNode node, XPathEx xPathEx)
        {
            if ((node == null) || (xPathEx == null)) return null;
            return node.XPathEvaluate(xPathEx.XPath, xPathEx.Namespaces.Manager);
        }
    }//EOC

    public class XPathEx
    {
        public XPathEx() 
        {
            this.Namespaces = new XPathNamespaces();
            this.XPath = string.Empty;
        }

        public string XPath { get; private set; }
        public XPathNamespaces Namespaces { get; private set; }

        public string ExtendedXPath
        { 
            get 
            {
                return this.Namespaces.ToString() + this.XPath;
            }
            set
            {
                if (value == null) value = string.Empty;
                string xpath = value;
                string namespaces = string.Empty;
                if (!string.IsNullOrEmpty(value) && (value[0] == '{'))
                {
                    var parts = value.Split(new char[] { '}' }, 2);
                    namespaces = parts[0].Substring(1);
                    xpath = parts[1];
                }
                this.Namespaces = namespaces;
                this.XPath = xpath;
            }
        }        

        public string NamespacesName 
        {
            get 
            {
                return this.Namespaces.NamespacesName;
            }
        }        

        public override string ToString()
        {
            return "{" + this.ExtendedXPath + "}";
        }

        #region equality

        public override bool Equals(object obj)
        {
            var other = obj as XPathEx;
            if (obj == null) return base.Equals(obj);
            return this.ExtendedXPath.Equals(other.ExtendedXPath);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator !=(XPathEx a, XPathEx b)
        {
            return !(a == b);
        }

        public static bool operator ==(XPathEx a, XPathEx b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.ExtendedXPath.Equals(b.ExtendedXPath);
        }

        #endregion equality


        public static implicit operator XPathEx(string xPathEx)
        {
            return new XPathEx { ExtendedXPath = xPathEx };
        }

    }//EOC

    public class XPathNamespaces
    {
        public XPathNamespaces()
        {
            this.Manager = new XmlNamespaceManager(new NameTable());
        }

        public XmlNamespaceManager Manager { get; private set; }

        public string NamespacesName
        {
            get
            {
                var sb = new StringBuilder();
                if (this.Manager != null)
                {
                    foreach (var ns in this.Manager.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml))
                    {
                        sb.Append(ns.Key).Append(',').Append(ns.Value).Append(',');
                    }
                    if (sb.Length > 1)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                }
                return sb.ToString();
            }
            set
            {
                this.Manager = null;
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                value = value.Replace("\r", string.Empty);
                value = value.Replace("\n", string.Empty);
                value = value.Replace("\t", string.Empty);
                value = value.Replace(" ", string.Empty);
                var nss = value.Split(',');
                var man = new XmlNamespaceManager(new NameTable());
                for (int i = 0; i < nss.Length; i = i + 2)
                {
                    man.AddNamespace(nss[i], nss[i + 1]);
                }
                this.Manager = man;
            }
        }             

        public override string ToString()
        {
            return "{" + this.NamespacesName + "}";
        }

        #region equality

        public override bool Equals(object obj)
        {
            var other = obj as XPathNamespaces;
            if (obj == null) return base.Equals(obj);
            return this.NamespacesName.Equals(other.NamespacesName);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator !=(XPathNamespaces a, XPathNamespaces b)
        {
            return !(a == b);
        }

        public static bool operator ==(XPathNamespaces a, XPathNamespaces b) 
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.NamespacesName.Equals(b.NamespacesName);
        }

        #endregion equality

        public static implicit operator XPathNamespaces(string xPathNamespaces)
        {
            return new XPathNamespaces { NamespacesName = xPathNamespaces };
        }

        public static XPathEx operator +(XPathNamespaces ns, string xpath)
        {
            return new XPathEx { ExtendedXPath = ns.ToString() + xpath };
        }
    }//EOC    

}
