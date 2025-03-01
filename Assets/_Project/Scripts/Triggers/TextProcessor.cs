using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;


public static class TextProcessor
{
    private static readonly Dictionary<string, TokenType> Tokens;
    private static readonly Dictionary<string, int> OperationPriority;
    
    static TextProcessor()
    {
        Tokens = new Dictionary<string, TokenType>
        {
            { @"(^\()", TokenType.OpeningBracket },
            { @"(^[+\-*\/%])|\^|&|\||==|!=|>=|<=|>|<|>>|<<|!", TokenType.Operator },
            { @"(^\$[A-z]+[\w]*)", TokenType.Variable },
            { "(^,)", TokenType.Divider },
            { @"(^\))", TokenType.ClosingBracket },
            { @"(^[\d.\d]+)", TokenType.Value },
            { @"^(?!\\+)(?:""((?:\\\\""|[^""])*)""?)", TokenType.Value },
            { @"(^[\w!@#$^&{}[\]';:?.>,<""]+)", TokenType.Value }
        };
        OperationPriority = new Dictionary<string, int>
        {
            { ">>", -2 },
            { "<<", -2 },
            
            { "==", -1 },
            { "!=", -1 },
            { ">", -1 },
            { ">=", -1 },
            { "<", -1 },
            { "<=", -1 },
            
            { "!", 1 },
            { "&", 1 },
            { "|", 1 },
            { "^", 1 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "%", 2 }
        };
    }
    public static string Execute(string text, IDictionary<string, string> variables)
    {
        var tokens = Tokenize(text, variables);
        tokens = SortingStation(tokens);
        var engine = new Engine(variables);
        
        foreach (var token in tokens)
            switch (token.Type)
            {
                case TokenType.Variable:
                case TokenType.Value:
                    engine.Push(token);
                    break;
                case TokenType.Operator:
                {
                    var b = engine.Pop();
                    switch (token.Value)
                    {
                        case "+":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue + bValue).ToString(CultureInfo.InvariantCulture)
                                    : aValue + b);
                            else
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue) 
                                    ? a + bValue 
                                    : a + b);
                            break;
                        }
                        case "-":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue - bValue).ToString(CultureInfo.InvariantCulture)
                                    : a.Replace(b, ""));
                            else
                                engine.Push(a.Replace(b, ""));
                            break;
                        }
                        case "*":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue * bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("Number on string multiplication is not allowed"));
                            else  
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                ? string.Join("", Enumerable.Repeat(a, (int) bValue))
                                : throw new ArithmeticException("String on string multiplication is not allowed"));
                            break;
                        }
                        case "/":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue / bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String division is not allowed"));
                            else throw new ArithmeticException("String division is not allowed");
                            break;
                        }
                        case "%":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue % bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case "&":
                        {
                            var a = engine.Pop();
                            if (long.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(long.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue & bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String and is not allowed"));
                            else throw new ArithmeticException("String and is not allowed");
                            break;
                        }
                        case ">>":
                        {
                            var a = engine.Pop();
                            if (int.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(int.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue >> bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String right shift is not allowed"));
                            else throw new ArithmeticException("String right shift is not allowed");
                            break;
                        }
                        case "<<":
                        {
                            var a = engine.Pop();
                            if (int.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(int.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue << bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String left shift is not allowed"));
                            else if (bool.TryParse(a, out var aValue1))
                                engine.Push(bool.TryParse(b, out var bValue1)
                                    ? (aValue1 && bValue1).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String bool and is not allowed"));
                            else throw new ArithmeticException("String left shift is not allowed");
                            break;
                        }
                        case "|":
                        {
                            var a = engine.Pop();
                            if (long.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(long.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue | bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String or is not allowed"));
                            else if (bool.TryParse(a, out var aValue1))
                                engine.Push(bool.TryParse(b, out var bValue1)
                                    ? (aValue1 || bValue1).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String bool or is not allowed"));
                            else throw new ArithmeticException("String or is not allowed");
                            break;
                        }
                        case "^":
                        {
                            var a = engine.Pop();
                            if (long.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(long.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue ^ bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String xor is not allowed"));
                            else throw new ArithmeticException("String xor is not allowed");
                            break;
                        }
                        case "==":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue == bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else if (bool.TryParse(a, out var aValue1))
                                engine.Push(bool.TryParse(b, out var bValue1)
                                    ? (aValue1 == bValue1).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case "!=":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue != bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else if (bool.TryParse(a, out var aValue1))
                                engine.Push(bool.TryParse(b, out var bValue1)
                                    ? (aValue1 != bValue1).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case ">=":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue >= bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case "<=":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue <= bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case ">":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue > bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case "<":
                        {
                            var a = engine.Pop();
                            if (double.TryParse(a, NumberStyles.Any, CultureInfo.InvariantCulture, out var aValue))
                                engine.Push(double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue)
                                    ? (aValue < bValue).ToString(CultureInfo.InvariantCulture)
                                    : throw new ArithmeticException("String remainder is not allowed"));
                            else throw new ArithmeticException("String remainder is not allowed");
                            break;
                        }
                        case "!":
                        {
                            if (double.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue))
                                engine.Push((-bValue).ToString(CultureInfo.InvariantCulture));
                            else if (long.TryParse(b, NumberStyles.Any, CultureInfo.InvariantCulture, out var bValue1))
                                engine.Push((~bValue1).ToString(CultureInfo.InvariantCulture));
                            else if (bool.TryParse(b, out var bValue2))
                                engine.Push((!bValue2).ToString(CultureInfo.InvariantCulture));
                            else throw new ArithmeticException("String not is not allowed");
                            break;
                        }
                    }
                    break;
                }
                case TokenType.Divider:
                case TokenType.OpeningBracket:
                case TokenType.ClosingBracket:
                default:
                    throw new ArgumentOutOfRangeException();
            }

        var output = "";
        while (engine.TryPop(out var value)) output = $"{value} " + output;
        return output[..^1];
    }

    private static List<Token> Tokenize(string text, IDictionary<string, string> variables) {
        var tokens = new List<Token>();
        while (0 != text.Length)
        {
            var foundMatch = false;
            text = Regex.Replace(text, "^\\s*", "");
            
            foreach (var (token, type) in Tokens) {
                var match = Regex.Match(text, token);
                if (!match.Success) continue;

                var name = match.Groups[1].Value;
                if (type == TokenType.Variable)
                {
                    name = name[1..];
                    if (!variables.ContainsKey(name))
                        throw new ArgumentException("Variable is not defined");
                }

                tokens.Add(new Token(type, name));
                text = Regex.Replace(text, token, "");
                foundMatch = true;
                text = Regex.Replace(text, "^\\s*", "");
                break;
            }

            if (!foundMatch)
                throw new FormatException($"Code is invalid! Parsing error on line:\n{text}");
        }
        return tokens;
    }

    private static List<Token> SortingStation(List<Token> tokens)
    {
        var output = new List<Token>();
        var stack = new Stack<Token>();
        
        foreach (var token in tokens) 
            switch (token.Type)
            {
                case TokenType.Variable:
                case TokenType.Value:
                {
                    output.Add(token);
                    break;
                }
                case TokenType.Function:
                {
                    stack.Push(token);
                    break;
                }
                case TokenType.Operator:
                {
                    while (stack.TryPeek(out var result) 
                           && result.Type == TokenType.Operator
                           && OperationPriority[result.Value] >= OperationPriority[token.Value])
                        output.Add(stack.Pop());
                    stack.Push(token);
                    break;
                }
                case TokenType.Divider:
                {
                    while (true)
                    {
                        if (!stack.TryPeek(out var result))
                            throw new Exception("Code parsing error!");
                        if (result.Type == TokenType.OpeningBracket) break;
                        output.Add(stack.Pop());
                    }

                    break;
                }
                case TokenType.OpeningBracket:
                {
                    stack.Push(token);
                    break;
                }
                case TokenType.ClosingBracket:
                {
                    Token result;
                    while (true)
                    {
                        if (!stack.TryPeek(out result))
                            throw new Exception("Code parsing error!");
                        if (result.Type == TokenType.OpeningBracket) break;
                        output.Add(stack.Pop());
                    }
                    stack.Pop();
                    if (stack.TryPeek(out result) && result.Type == TokenType.Function) 
                        output.Add(stack.Pop());
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
        while (stack.TryPop(out var result))
            if (result.Type == TokenType.OpeningBracket) 
                throw new FormatException("Code parsing error!");
            else output.Add(result);
        return output;
    }
    
    internal readonly struct Token
    {
        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; }
        public string Value { get; }
    }
    internal enum TokenType
    {
        Variable,
        Value,
        Function,
        Operator,
        Divider,
        OpeningBracket,
        ClosingBracket
    }

    public class Engine
    {
        private readonly Stack<Token> _stack;
        private readonly IDictionary<string, string> _variables;

        internal Engine(IDictionary<string, string> variables)
        {
            _variables = variables;
            _stack = new Stack<Token>();
        }

        internal void Push(Token token) => _stack.Push(token);
        public void Push(string value) => _stack.Push(new Token(TokenType.Value, value));
        public string Pop()
        {
            var token = _stack.Pop();
            return token.Type == TokenType.Variable ? _variables[token.Value] : token.Value;
        }
        public bool TryPop(out string? value)
        {
            if (!_stack.TryPop(out var token))
            {
                value = null;
                return false;
            }
            value = token.Type == TokenType.Variable ? _variables[token.Value] : token.Value; 
            return true;
        }
    }
}