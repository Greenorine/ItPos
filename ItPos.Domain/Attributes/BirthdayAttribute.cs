using System.ComponentModel.DataAnnotations;

namespace ItPos.Domain.Attributes;

public class BirthdayAttribute : RangeAttribute
{
    public BirthdayAttribute() : base(typeof(DateTime),
        DateTime.Now.AddYears(-120).ToShortDateString(),
        DateTime.Now.AddYears(-17).ToShortDateString())
    {
    }
}