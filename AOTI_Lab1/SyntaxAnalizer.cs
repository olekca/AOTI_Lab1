using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTI_Lab1
{
    class SyntaxAnalizer
    {
        AnalizerNode root = new ForStatement();
        public void Analize(Queue<OutputToken> queue)
        {
            try
            {
                root.Analize(queue);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Analysis finished successfully");
            }
            
        }
        public void PrintTree()
        {
            root.PrintNode();
        }
    }
    class AnalizerNode
    {
        protected bool IsFinalNode = false;
        protected string NodeDesc;
        protected AnalizerNode ChosenNode;
        protected List<OutputToken> Syntax = new List<OutputToken> { };
        protected List<AnalizerNode> Nodes = new List<AnalizerNode> { };        
        
        public void AddNodes(params AnalizerNode[] args)
        {
            foreach(AnalizerNode n in args)
            {
                this.Nodes.Add(n);
            }
        }
        public void AddSyntax(params TokenType[] args)
        {
            foreach (TokenType t in args)
            {
                this.Syntax.Add(new OutputToken(t));
            }
        }
        public void Analize(Queue<OutputToken> queue)
        {
            CheckSyntax(queue);
            GoToNextNode(queue);
        }
        protected virtual void CheckSyntax(Queue<OutputToken> queue)
        {
            foreach(OutputToken t in Syntax)
            {
                OutputToken q = queue.Dequeue();
                if (t.TokenType == q.TokenType)
                {
                    t.Value = q.Value;
                }
                else
                {
                    throw new Exception("Wrong syntax");
                }
            }
        }
        protected virtual void GoToNextNode(Queue<OutputToken> queue)
        {
            if (queue.Count() == 0 && ! IsFinalNode)
            {
                throw new Exception("Syntax ends before expected");
            }
            if (IsFinalNode == true)
            {
                return;
            }
            else
            {
                foreach(AnalizerNode n in Nodes)
                {
                    if (n.GetFirstToken() == queue.Peek().TokenType)
                    {
                        this.ChosenNode = n; 
                        n.Analize(queue);
                        return;
                    }
                }
                throw new Exception("No appropriate syntax variant");
            }
        }
        public TokenType GetFirstToken()
        {
            return this.Syntax[0].TokenType;
        }
        public TokenType GetSecondToken()
        {
            return this.Syntax[1].TokenType;
        }
        public void PrintNode()
        {
            Console.WriteLine(NodeDesc);
            foreach( OutputToken t in Syntax)
            {
                Console.WriteLine($"{t.TokenType}:    {t.Value}");
            }
            if (ChosenNode != null)
            {
                ChosenNode.PrintNode();
            }
            else
            {
                if (IsFinalNode)
                {
                    Console.WriteLine("End of syntax");//we need to choose something NORMAL goddamn it
                }
                
            }
        }

    }

    
    class ForStatement:AnalizerNode
    {
        public ForStatement()
        {            
            AddSyntax(TokenType.For);
            NodeDesc = "For Statement";
            AddNodes(new OpenParenthesis());
        }
    }
    class OpenParenthesis : AnalizerNode
    {
        public OpenParenthesis()
        {
            AddSyntax(TokenType.OpenParenthesis);
            NodeDesc = "Open parenthesis";
            AddNodes(new ArrayVariable(), new IntVariable(), new FloatVariable(), new DoubleVariable(), new Semicolon1(TokenType.IntNumber));

        }
    }
    class ArrayVariable :AnalizerNode
    {
        public ArrayVariable()
        {
            AddSyntax(TokenType.ArrayElement, TokenType.Equals, TokenType.IntNumber);
            NodeDesc = "Set value to array element";
            AddNodes(new Semicolon1(TokenType.IntNumber, TokenType.ArrayElement));
        }
    }
    class IntVariable : AnalizerNode
    {
        public IntVariable()
        {
            AddSyntax(TokenType.Int,TokenType.Var, TokenType.Equals, TokenType.IntNumber);
            NodeDesc = "Set value to integer";
            AddNodes(new Semicolon1(TokenType.IntNumber));
        }
    }

    class FloatVariable : AnalizerNode
    {
        public FloatVariable()
        {
            AddSyntax(TokenType.Float, TokenType.Var, TokenType.Equals, TokenType.FloatNumber);
            NodeDesc = "Set value to Float";
            AddNodes(new Semicolon1(TokenType.FloatNumber));
        }
    }
    class DoubleVariable : AnalizerNode
    {
        public DoubleVariable()
        {
            AddSyntax(TokenType.Double, TokenType.Var, TokenType.Equals, TokenType.FloatNumber);
            NodeDesc = "Set value to Double";
            AddNodes(new Semicolon1(TokenType.FloatNumber));
        }
    }
    class Semicolon : AnalizerNode
    {
        protected override void GoToNextNode(Queue<OutputToken> queue)
        {
            if (queue.Count() <= 1 && !IsFinalNode)
            {
                throw new Exception("Syntax ends before expected");
            }
            if (IsFinalNode == true)
            {
                return;
            }
            else
            {
                foreach (AnalizerNode n in Nodes)
                {
                    TokenType t = queue.Peek().TokenType;
                    t = queue.ElementAt(1).TokenType;
                    if (n.GetFirstToken() == queue.Peek().TokenType && n.GetSecondToken() == queue.ElementAt(1).TokenType)
                    {
                        this.ChosenNode = n;
                        n.Analize(queue);
                    }
                }
                throw new Exception("No appropriate syntax variant");
            }
        }
    }
    class Semicolon1 : Semicolon
    {
        
        public Semicolon1(TokenType valueType, TokenType variableType=TokenType.Var)
        {
            AddSyntax(TokenType.Semicolon);
            NodeDesc = "Semicolon";
            AddNodes(new ComparisonExpression(TokenType.LessThan, valueType, variableType),
                new ComparisonExpression(TokenType.LessOrEven, valueType, variableType),
                new ComparisonExpression(TokenType.GreaterThan, valueType, variableType),
                new ComparisonExpression(TokenType.GreaterOrEven, valueType, variableType),
                new ComparisonExpression(TokenType.IsEqual, valueType, variableType),
                new ComparisonExpression(TokenType.NotEqual, valueType, variableType),
                new Semicolon2(valueType, variableType));
        }
    }

    class ComparisonExpression : AnalizerNode
    {
        public ComparisonExpression(TokenType comparison, TokenType dataType,TokenType variableType)
        {
            AddSyntax(variableType, comparison, dataType);
            NodeDesc = $"Compare variable with {comparison} operator to {dataType} value" ;
            AddNodes(new Semicolon2(dataType, variableType));
        }
    }
    class Semicolon2 : Semicolon
    {
        public Semicolon2(TokenType valueType, TokenType variableType)
        {
            AddSyntax(TokenType.Semicolon);
            NodeDesc = "Semicolon";
            AddNodes(new MathExpression(TokenType.Minus, valueType, variableType),
                new MathExpression(TokenType.Plus, valueType, variableType),
                new MathExpression(TokenType.Divide, valueType, variableType),
                new MathExpression(TokenType.Multiply, valueType, variableType),
                new CloseParenthesis());
        }
    }
    class MathExpression : AnalizerNode
    {
        public MathExpression(TokenType mathExp, TokenType dataType, TokenType variableType)
        {
            AddSyntax(variableType, mathExp, TokenType.Equals, dataType);
            NodeDesc = $"Compare variable with {mathExp} operator to {dataType} value";
            AddNodes(new CloseParenthesis());
        }
    }



    class CloseParenthesis : AnalizerNode
    {
        public CloseParenthesis()
        {
            AddSyntax(TokenType.CloseParenthesis);
            NodeDesc = "Close parenthesis";
            AddNodes(new BracesNode());
        }
    }
    class BracesNode : AnalizerNode
    {
        public BracesNode()
        {
            IsFinalNode = true;
            AddSyntax(TokenType.OpenBrace);
            NodeDesc = "Braces and everything inside";
        }

        protected override void CheckSyntax(Queue<OutputToken> queue)
        {
            OutputToken q = queue.Dequeue();//because we check type of first token in list before
                                            //transition to this node in previous,
                                            //we can skip checking for type here
            Syntax[0].Value = q.Value;
            do
            {
                if (queue.Count() == 0)
                {
                    throw new Exception("Braces now closed");
                }
                q = queue.Dequeue();
                Syntax.Add(q);

            } while (q.TokenType != TokenType.CloseBrace);

        }


    }

}
