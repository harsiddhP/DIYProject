using System;
using System.Collections.Generic;
using System.Text;

using Amazon.Lambda.LexEvents;

namespace DIYProject
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ViolationSlot { get; }
        public LexResponse.LexMessage Message { get; }

        public ValidationResult(bool isValid, string violationSlot, string message)
        {
            this.IsValid = isValid;
            this.ViolationSlot = violationSlot;

            if (!string.IsNullOrEmpty(message))
            {
                this.Message = new LexResponse.LexMessage { ContentType = "PlainText", Content = message };
            }
        }

        public static readonly ValidationResult VALID_RESULT = new ValidationResult(true, null, null);
        public static readonly ValidationResult VALID_FAIL = new ValidationResult(true, null, null);

    }
}
