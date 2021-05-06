using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace LibraryManagementSystem
{
    //  Проверка некоторых входных данных
    static class ValidatorHelper
    {
        public static bool IsLettersOnly(string input)
        {
            foreach (char c in input)
                if (!Char.IsLetter(c))
                    return false;
            return true;
        }

        public static bool IsLettersOrNumbersOnly(string input)
        {
            foreach (char c in input)
                if (!Char.IsLetterOrDigit(c))
                    return false;
            return true;
        }

        public static bool IsNumbersOnly(string input)
        {
            int tmp;
            return int.TryParse(input, out tmp);
        }

        //  Проверяет наличие действительного PPSN
        public static bool IsValidPPSN(string input)
        {
            //  должна быть длина 9
            if (input.Length != 9)
                return false;
            //  первые 7 символов должны быть цифрами
            if (!IsNumbersOnly(input.Substring(0, 7)))
                return false;
            //  далее 8 - символы должны быть буквой
            if (!Char.IsLetter(input[7]))
                return false;
            //  9-й символ-это либо буква, либо пробел
            return Char.IsLetter(input[8]) || Char.IsWhiteSpace(input[8]);
        }

        //  Не полная действительная проверка ISBN, просто быстрая проверка
        public static bool MeetsSlimlinedISBNRequirements(string input)
        {
            bool validLength = input.Length == 10 || input.Length == 13;
            return validLength && (input.Count(x => Char.IsNumber(x) || x == '-' || x == ' ') == input.Length);
        }

        public static bool IsLettersOrWhitespaceOnly(string input)
        {
            foreach (char c in input)
                if (!Char.IsLetter(c) || !Char.IsWhiteSpace(c))
                    return false;
            return true;
        }
        
        public static bool IsEmailValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
