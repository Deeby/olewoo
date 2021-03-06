using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using olewoo_interop;

namespace Org.Benf.OleWoo.Typelib
{
    internal class OWTypeDef : ITlibNode
    {
        private readonly ITypeInfo _ti;
        private readonly TypeAttr _ta;
        private readonly string _name;

        public OWTypeDef(ITlibNode parent, ITypeInfo ti, TypeAttr ta)
        {
            Parent = parent;
            _ta = ta;
            _ti = ti;

            _ti.GetRefTypeInfo(_ta.tdescAlias.hreftype, out var oti);
            _name = oti.GetName() + " " + ti.GetName();        
        }
        public override string Name => "typedef " + _name;
        public override string ShortName => _name;
        public override string ObjectName => _name + "#i";

        public override bool DisplayAtTLBLevel(ICollection<string> interfaceNames)
        {
            return true;
        }
        public override int ImageIndex => (int)ImageIndices.idx_typedef;
        public override ITlibNode Parent { get; }

        public override List<ITlibNode> GenChildren()
        {
            var res = new List<ITlibNode>();
            _ti.GetRefTypeInfo(_ta.tdescAlias.hreftype, out var oti);
            CommonBuildTlibNode(this, oti, false, false, res);
            return res;
        }
        public override void BuildIDLInto(IDLFormatter ih)
        {
            ih.AppendLine("typedef [public] " + _name + ";");
        }
    }
}