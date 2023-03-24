using System;
using System.Collections.Generic;

namespace AOTI_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleRegexTokenizer t = new SimpleRegexTokenizer();
            IEnumerable<OutputToken> list = t.Tokenize("for (qwe[1] = 0; qwe[1]<5; qwe[1]+=1){ int v = 9; }");
            foreach(OutputToken toc in list)
            {
                Console.WriteLine(toc.Value+"   "+toc.TokenType.ToString());

            }
            Queue<OutputToken> q = new Queue<OutputToken>(list);
            SyntaxAnalizer s = new SyntaxAnalizer();
            s.Analize(q);
            s.PrintTree();
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
        Invalid,
        Increment,
        IsEqual,
        NotEqual


    }

   
    /*
    Мова, що представляє оператор циклу for з тілом циклу у вигляді оператора
присвоювання зі змінною або елементом масиву в лівій частині оператора
присвоювання та арифметичним виразом (складення, віднімання, множення, поділ,
проста змінна, константа) у правій.
     */



}
