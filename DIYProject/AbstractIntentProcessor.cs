using Amazon.Lambda.Core;
using Amazon.Lambda.LexEvents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DIYProject
{
    public abstract class AbstractIntentProcessor : IIntentProcessor
    {
        internal const string MESSAGE_CONTENT_TYPE = "PlainText";
        public abstract LexResponse Process(LexEvent lexEvent, ILambdaContext context);

        protected string SerializeReservation(SetAlarmTime order)
        {
            return JsonSerializer.Serialize(order, new JsonSerializerOptions
            {
                IgnoreNullValues = true
            });
        }

        protected SetAlarmTime DeserializeReservation(string json)
        {
            return JsonSerializer.Deserialize<SetAlarmTime>(json);
        }

        protected LexResponse Close(IDictionary<string, string> sessionAttributes, string fulfillmentState, LexResponse.LexMessage message)
        {
            return new LexResponse
            {
                SessionAttributes = sessionAttributes,
                DialogAction = new LexResponse.LexDialogAction
                {
                    Type = "Close",
                    FulfillmentState = fulfillmentState,
                    Message = message
                }
            };
        }

        protected LexResponse Delegate(IDictionary<string, string> sessionAttributes, IDictionary<string, string> slots)
        {
            return new LexResponse
            {
                SessionAttributes = sessionAttributes,
                DialogAction = new LexResponse.LexDialogAction
                {
                    Type = "Delegate",
                    Slots = slots
                }
            };
        }

        protected LexResponse ElicitSlot(IDictionary<string, string> sessionAttributes, string intentName, IDictionary<string, string> slots, string slotToElicit, LexResponse.LexMessage message)
        {
            return new LexResponse
            {
                SessionAttributes = sessionAttributes,
                DialogAction = new LexResponse.LexDialogAction
                {
                    Type = "ElicitSlot",
                    IntentName = intentName,
                    Slots = slots,
                    SlotToElicit = slotToElicit,
                    Message = message
                }
            };
        }

        protected LexResponse ConfirmIntent(IDictionary<string, string> sessionAttributes, string intentName, IDictionary<string, string> slots, LexResponse.LexMessage message)
        {
            return new LexResponse
            {
                SessionAttributes = sessionAttributes,
                DialogAction = new LexResponse.LexDialogAction
                {
                    Type = "ConfirmIntent",
                    IntentName = intentName,
                    Slots = slots,
                    Message = message
                }
            };
        }

    }
}
