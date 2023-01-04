using System.ComponentModel.DataAnnotations;

namespace Backend_IESIFA.Validations
{
    public class RangoCalificacionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            decimal calificacion = Convert.ToDecimal(value);

            if (calificacion > 10)
            {
                return new ValidationResult("La calificación no deben ser mayor a 10.");
            }

            if (calificacion < 0)
            {
                return new ValidationResult("La calificación no debe ser menor a 0.");
            }

            return ValidationResult.Success;
        }

    }
}
