using System;
using System.Collections.Generic;
using System.Text;

namespace IMCTaxService.Proxy
{
    public interface IFactory<Obj, Input>
    {
        Obj GetInstance(Input input);
    }
}
