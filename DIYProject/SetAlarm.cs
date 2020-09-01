using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static DIYProject.SetAlarmTime;

namespace DIYProject
{
    public class SetAlarm : AbstractIntentProcessor
    {
        public const string TYPE_SLOT = "TimeType";
        public const string ALARMTIME_SLOT = "AlarmTime";

        TimeTypes _chosenTimeType = TimeTypes.Null;
        public override LexResponse Process(LexEvent lexEvent, ILambdaContext context)
        {
            IDictionary<string, string> slots = lexEvent.CurrentIntent.Slots;
            IDictionary<string, string> sessionAttributes = lexEvent.SessionAttributes ?? new Dictionary<string, string>();

            //if slots are empty returns the delegate
            if (slots.All(x => x.Value == null))
            {
                
            }

            if(slots[TYPE_SLOT] != null)
            {
                var validateTimeType = ValidateTimeOfTheDay(slots[TYPE_SLOT]);
                if (!validateTimeType.IsValid)
                {                    
                    slots[validateTimeType.ViolationSlot] = null;
                    //return ElicitSlot(sessionAttributes, lexEvent.CurrentIntent.Name, slots, validateFlowerType.ViolationSlot, validateFlowerType.Message);                    
                }
            }

            SetAlarmTime setAlarmTime = SetValidedAlarm(slots);

            if (string.Equals(lexEvent.InvocationSource, "DialogCodeHook", StringComparison.Ordinal))
            {
              
                //var validateResult = Validate(order);
            }
            return Close(
                        sessionAttributes,
                        "Fulfilled",
                        new LexResponse.LexMessage
                        {
                            ContentType = MESSAGE_CONTENT_TYPE,
                            Content = String.Format("Alarm is set for {0} {1}.", setAlarmTime.AlarmTime, setAlarmTime.TimeType.ToString())
                        }
                    );
        }

        private ValidationResult ValidateTimeOfTheDay(string timeString)
        {
            bool isTimeTypeValid = Enum.IsDefined(typeof(TimeTypes), timeString.ToUpper());

            if (isTimeTypeValid && Enum.TryParse(typeof(TimeTypes), timeString, true, out object timeTypeEnumObj))
            {
                _chosenTimeType = (TimeTypes)timeTypeEnumObj;
                return ValidationResult.VALID_RESULT;
            }
            else
            {
                return new ValidationResult(false, TYPE_SLOT, String.Format("We cannot set time for {0} at this time. Please try again later.", timeString));
            }
        }

        private SetAlarmTime SetValidedAlarm(IDictionary<string, string> slots)
        {
            SetAlarmTime setTime = new SetAlarmTime
            {
                TimeType = _chosenTimeType,
                AlarmTime = slots.ContainsKey(ALARMTIME_SLOT) ? slots[ALARMTIME_SLOT] : null,
            };
            return setTime;
        }
    }
}
