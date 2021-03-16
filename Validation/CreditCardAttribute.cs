using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.ComponentModel;

namespace CWTDesktopDatabase.Validation
{
    /// ASP.NET MVC 3 Credit Card Validator Attribute
    /// by Ben Cull - 4 November 2010
    /// 
    /// With special thanks to:
    /// Thomas @ Orb of Knowledge - http://orb-of-knowledge.blogspot.com/2009/08/extremely-fast-luhn-function-for-c.html 
    /// For the Extremely fast Luhn algorithm implementation
    /// 
    /// And Paul Ingles - http://www.codeproject.com/KB/validation/creditcardvalidator.aspx
    /// For a timeless blog post on credit card validation

    public class CreditCardAttribute : ValidationAttribute, IClientValidatable
    {

        //private static string allowValidCardsOnly = "yes";

        public CreditCardAttribute()
        {
           
        }

        //private string _CreditCardBehaviour;
        /*public string CreditCardBehaviour
        {
            get { return _CreditCardBehaviour; }
            set { _CreditCardBehaviour = value; }
        }

        public string PropertyToCheck
        {
            get;
            private set;
        }

         override bool IsValid(object value)
        {
            var number = Convert.ToString(value);

            if (String.IsNullOrEmpty(number))
                return true;

            //return IsValidType(number, _cardTypes) && IsValidNumber(number);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            //object propertyValue = properties.Find(PropertyToCheck, true ignoreCase ).GetValue(value);
            if (propertyValue == "False")
            {
                return IsValidNumber(number);
            }
            else
            {
                return !IsValidNumber(number);
            }
        }*/


        /// <summary>
        /// Override IsValid 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)  
        {
            /*
             * canHaveRealCreditCardsFlag is true in Production Environments. This means that both valid and invalid cards are alloed
             * canHaveRealCreditCardsFlag is false in Test Environments. This means that only invalid cards are alloed
             * 
             * In Production Environment, we must show a waring if number is invalid, but allow the user to continue anyway
             * We do this be using "WarningShownFlag" property on the CreditCard Class 
             * (should move "WarningShownFlag" & "CanHaveRealCreditCardsFlag" to parent ViewModel when time permits)
             * 
             * CreditCard.WarningShownFlag will be set to False when CanHaveRealCreditCardsFlag=true
             * CreditCard.WarningShownFlag will be set to True when CanHaveRealCreditCardsFlag=false
             */
            //Get PropertyInfo Object  
            //var basePropertyInfo = validationContext.ObjectType.GetProperty(PropertyToCheck);  
        
            //Get Value of the property  
            //CreditCard cc = basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

           
			if (value == null) {
				return null; 
			}

            var containerType = validationContext.ObjectInstance.GetType();
            var canHaveRealCreditCardsFlagProperty = containerType.GetProperty("CanHaveRealCreditCardsFlag");

           // var v = containerType.GetProperty("ValidateCreditCardNumber");
            //bool validate =  (bool)v.GetValue(validationContext.ObjectInstance, null);
           
            bool WarningShown = true;
            bool CanHaveRealCreditCardsFlag = (bool)canHaveRealCreditCardsFlagProperty.GetValue(validationContext.ObjectInstance, null);
            if(CanHaveRealCreditCardsFlag){
                var warningShownProperty = containerType.GetProperty("WarningShownFlag");
                WarningShown = (bool)warningShownProperty.GetValue(validationContext.ObjectInstance, null);
             }

            string thisCreditCardNumber = value.ToString();
            bool validData = true;

            //this is for matching masked numbers like ************1234
            //when editing cards we need to check for this as it is a required field
            Match match = Regex.Match(thisCreditCardNumber, @"^(\*){8,12}[0-9]{4}$", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                //We are editing a Card
                validData = true;
            }
            else
            {
                if (CanHaveRealCreditCardsFlag)
                {
                    if (WarningShown)
                    {
                        //Second Submit - User has been warned that the Number is InValid, now they can submit If they want
                        validData = true;
                    }
                    else
                    {
                        //First Submit - Number has not been Checked, check for validity
                        validData = IsValidNumber(thisCreditCardNumber);
                    }
                }
                else
                {
                    //User Is not Allowed to enter a Valid Card Number
                    validData = !IsValidNumber(thisCreditCardNumber);
                }
            }
            //Actual comparision  
            if (!validData)  
            {
                var message = FormatErrorMessage2(validationContext.DisplayName, CanHaveRealCreditCardsFlag);  
                return new ValidationResult(message);  
            }  
   
            //Default return - This means there were no validation error  
            return null;  
        }

        public string FormatErrorMessage2(string name, bool canHaveRealCreditCardsFlag)
        {
            if (canHaveRealCreditCardsFlag)
            {
                return "NOTE: You are adding an invalid credit card number, you may continue";
            }
            else
            {
                return "Valid Credit Card Numbers are not allowed.";
            }
        }
        /*
        public enum CardType
        {
            Unknown = 1,
            Visa = 2,
            MasterCard = 4,
            Amex = 8,
            Diners = 16,

            All = CardType.Visa | CardType.MasterCard | CardType.Amex | CardType.Diners,
            AllOrUnknown = CardType.Unknown | CardType.Visa | CardType.MasterCard | CardType.Amex | CardType.Diners
        }
       


        private bool IsValidType(string cardNumber, CardType cardType)
        {
            // Visa
            if (Regex.IsMatch(cardNumber, "^(4)")
                && ((cardType & CardType.Visa) != 0))
                return cardNumber.Length == 13 || cardNumber.Length == 16;

            // MasterCard
            if (Regex.IsMatch(cardNumber, "^(51|52|53|54|55)")
                && ((cardType & CardType.MasterCard) != 0))
                return cardNumber.Length == 16;

            // Amex
            if (Regex.IsMatch(cardNumber, "^(34|37)")
                && ((cardType & CardType.Amex) != 0))
                return cardNumber.Length == 15;

            // Diners
            if (Regex.IsMatch(cardNumber, "^(300|301|302|303|304|305|36|38)")
                && ((cardType & CardType.Diners) != 0))
                return cardNumber.Length == 14;

            //Unknown
            if ((cardType & CardType.Unknown) != 0)
                return true;

            return false;
        }
        */
        private bool IsValidNumber(string number)
        {
            int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
            int checksum = 0;
            char[] chars = number.ToCharArray();
            for (int i = chars.Length - 1; i > -1; i--)
            {
                int j = ((int)chars[i]) - 48;
                checksum += j;
                if (((i - chars.Length) % 2) == 0)
                    checksum += DELTAS[j];
            }

            return ((checksum % 10) == 0);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {

            /*var parentType = metadata.ContainerType;
            //var parentMetaData = ModelMetadataProviders.Current.GetMetadataForType(null, parentType);

            var parentMetaData = ModelMetadataProviders.Current.GetMetadataForProperties(context.Controller.ViewData.Model, parentType);

            var otherProperty = parentMetaData.FirstOrDefault(p =>
                p.PropertyName == "OriginalCreditCardNumber");

            var otherValue = otherProperty.Model;

            */
           
                yield return new ModelClientValidationRule
                {
                    ErrorMessage = this.ErrorMessage,
                    ValidationType = "creditcard"
                };
           
        }
    }
}