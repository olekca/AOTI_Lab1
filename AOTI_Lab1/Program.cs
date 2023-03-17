using System;
using System.Collections.Generic;

namespace AOTI_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleRegexTokenizer t = new SimpleRegexTokenizer();
            IEnumerable<OutputToken> list = t.Tokenize("for (int i<=0; i<3; i+=1){kfnvkd/vfl;zd  sdlvmkdfv sksdlv}");
            foreach(OutputToken toc in list)
            {
                Console.WriteLine(toc.Value+"   "+toc.TokenType.ToString());

            }
            Console.WriteLine("Hello World!");
        }
    }
    public enum TokenType
    {
        For,
        OpenParenthesis,
        CloseParenthesis,
        Int,
        Float,
        Double,
        Var,
        Equals,
        IntNumber,
        FloatNumber,
        ArrayElement,
        Semicolon,
        LessThan,
        GreaterThan,
        LessOrEven,
        GreaterOrEven,
        Plus,
        Minus,
        Multiply,
        Divide,
        OpenBrace,
        CloseBrace,
        SequenceTerminator,
        Invalid


    }

   
    /*
    Мова, що представляє оператор циклу for з тілом циклу у вигляді оператора
присвоювання зі змінною або елементом масиву в лівій частині оператора
присвоювання та арифметичним виразом (складення, віднімання, множення, поділ,
проста змінна, константа) у правій.
     */



}
