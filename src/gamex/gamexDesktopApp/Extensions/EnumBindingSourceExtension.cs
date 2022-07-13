using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace gamexDesktopApp.Extensions;

[MarkupExtensionReturnType(typeof(Enum))]
public class EnumBindingSourceExtension : MarkupExtension
{
    public Type EnumType { get; private set; }

    public EnumBindingSourceExtension(Type enumType)
    {
        if (enumType == null || !enumType.IsEnum)
            throw new Exception("EnumType must not be null and of type Enum");

        EnumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enum.GetValues(EnumType);
    }
}