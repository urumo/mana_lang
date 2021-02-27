﻿namespace wc_test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using wave;
    using wave.emit;
    using wave.extensions;
    using wave.fs;
    using wave.stl;
    using wave.syntax;
    using Xunit;
    using Xunit.Abstractions;

    public class generator_test
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public generator_test(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test()
        {
            var module = new WaveModuleBuilder("xuy");
            var clazz = module.DefineClass("global::wave/lang/svack_pidars");
            clazz.Flags = ClassFlags.Public | ClassFlags.Static;
            var method = clazz.DefineMethod("insert_dick_into_svack", MethodFlags.Public,WaveTypeCode.TYPE_VOID.AsType(), ("x", WaveTypeCode.TYPE_STRING));
            method.Flags = MethodFlags.Public | MethodFlags.Static;
            var gen = method.GetGenerator();
            
            var l1 = gen.DefineLabel();
            var l2 = gen.DefineLabel();
            gen.Emit(OpCodes.ADD);
            gen.Emit(OpCodes.LDC_I4_S, 228);
            gen.Emit(OpCodes.ADD);
            gen.Emit(OpCodes.JMP_HQ, l2);
            gen.Emit(OpCodes.LDC_I4_S, 228);
            gen.Emit(OpCodes.ADD);
            gen.Emit(OpCodes.LDC_I4_S, 228);
            gen.Emit(OpCodes.LDC_I4_S, 228);
            gen.Emit(OpCodes.ADD);
            gen.Emit(OpCodes.JMP_HQ, l1);
            gen.UseLabel(l1);
            gen.Emit(OpCodes.SUB);
            gen.Emit(OpCodes.SUB);
            gen.UseLabel(l2);
            gen.Emit(OpCodes.SUB);
            gen.Emit(OpCodes.SUB);
            
            
            File.WriteAllText(@"C:\Users\ls-mi\Desktop\wave.il", 
                module.BakeDebugString());
            
            var asm = new InsomniaAssembly{Name = "woodo"};
            
            asm.AddSegment((".code", method.BakeByteArray()));
            
            InsomniaAssembly.WriteToFile(asm, new DirectoryInfo(@"C:\Users\ls-mi\Desktop\"));

        }

        [Fact]
        public void TestIL()
        {
            var module = new WaveModuleBuilder("xuy");
            var clazz = module.DefineClass("global::wave/lang/svack_pidars");
            clazz.Flags = ClassFlags.Public | ClassFlags.Static;
            var method = clazz.DefineMethod("insert_dick_into_svack", MethodFlags.Public, WaveTypeCode.TYPE_VOID.AsType(), ("x", WaveTypeCode.TYPE_STRING));
            method.Flags = MethodFlags.Public | MethodFlags.Static;
            var body = method.GetGenerator();
            
            body.Emit(OpCodes.LDC_I4_S, 1448);
            body.Emit(OpCodes.LDC_I4_S, 228);
            body.Emit(OpCodes.ADD);
            body.Emit(OpCodes.LDC_I4_S, 2);
            body.Emit(OpCodes.XOR);
            body.Emit(OpCodes.DUMP_0);
            body.Emit(OpCodes.LDF, "x");
            body.Emit(OpCodes.RET);


            var body_module = module.BakeByteArray();


            var asm = new InsomniaAssembly();
            
            asm.AddSegment((".code", body_module));
            
            InsomniaAssembly.WriteToFile(asm, new DirectoryInfo(@"C:\Users\ls-mi\Desktop\"));
        }
        [Fact]
        public void AST2ILTest()
        {
            var w = new WaveSyntax();
            var ast = w.CompilationUnit.ParseWave(
                " class Program { void main() { if(ze()) return x; else { return d();  } } }");

            var module = new WaveModuleBuilder("foo");

            foreach (var member in ast.Members)
            {
                if (member is ClassDeclarationSyntax classMember)
                {
                    var @class = module.DefineClass($"global::wave/lang/{classMember.Identifier}");

                    foreach (var methodMember in classMember.Methods)
                    {
                        var method = @class.DefineMethod(methodMember.Identifier, WaveTypeCode.TYPE_VOID.AsType());
                        var generator = method.GetGenerator();

                        foreach (var statement in methodMember.Body.Statements)
                        {
                            var st = statement;
                        }
                    }
                }
            }
        }
        
        [Fact]
        public void ReturnStatementCompilation1()
        {
            var ret = new ReturnStatementSyntax
            {
                Expression = new SingleLiteralExpressionSyntax(14.3f)
            };


            var actual = CreateGenerator();
            
            actual.EmitReturn(ret);
            
            var expected = CreateGenerator();

            expected.Emit(OpCodes.LDC_F4, 14.3f);
            expected.Emit(OpCodes.RET);
            
            
            Assert.Equal(expected.BakeByteArray(), actual.BakeByteArray());
        }
        
        [Fact]
        public void ReturnStatementCompilation2()
        {
            var ret = new ReturnStatementSyntax
            {
                Expression = new ExpressionSyntax("x")
            };


            var actual = CreateGenerator(("x", WaveTypeCode.TYPE_STRING));
            
            actual.EmitReturn(ret);
            
            var expected = CreateGenerator(("x", WaveTypeCode.TYPE_STRING));

            expected.Emit(OpCodes.LDF, new FieldName("x"));
            expected.Emit(OpCodes.RET);
            
            
            Assert.Equal(expected.BakeByteArray(), actual.BakeByteArray());
        }
        
        [Fact]
        public void ReturnStatementCompilation3()
        {
            var ret = new ReturnStatementSyntax
            {
                Expression = new ExpressionSyntax("x")
            };
            
            var actual = CreateGenerator();
            
            
            Assert.Throws<FieldIsNotDeclaredException>(() => actual.EmitReturn(ret));
        }
        
        
        [Fact]
        public void BuiltinGenTest()
        {
            var module = new WaveModuleBuilder(Guid.NewGuid().ToString());
            BuiltinGen.GenerateConsole(module);
            module.BakeByteArray();
            module.BakeDebugString();
        }
        
        public static ILGenerator CreateGenerator(params WaveArgumentRef[] args)
        {
            var module = new WaveModuleBuilder(Guid.NewGuid().ToString());
            var @class = new ClassBuilder(module, "foo/bar");
            var method = @class.DefineMethod("foo", WaveTypeCode.TYPE_VOID.AsType(), args);
            return method.GetGenerator();
        }
        [Fact]
        public void Fib()
        {
            /*let fib = fun (n) {
  if (n < 2) return n;
  return fib(n - 1) + fib(n - 2); 
}

let before = clock();
puts fib(40);
let after = clock();
puts after - before;*/
            long f(long n)
            {
                if (n == 0)
                {
                    return 0;
                }
                if (n == 1)
                {
                    return 1;
                }
                long first = 0;
                long second = 1;
                long nth = 1;
                for (long i = 2; i <= n; i++)
                {
                    nth = first + second;
                    first = second;
                    second = nth;
                }
                return nth;
            }
            
            var s = new Stopwatch();
            
            s.Start();
            var a = f(int.MaxValue / 2);
            s.Stop();
            _testOutputHelper.WriteLine($"{a}, {int.MaxValue / 2} {s.Elapsed.TotalMilliseconds / 1000f} seconds.");
        }
        
        [Fact]
        public void ManualGen()
        {
            var module = new WaveModuleBuilder("satl");
            var clazz = module.DefineClass("global::wave/lang/program");
            clazz.Flags = ClassFlags.Public | ClassFlags.Static;
            
            
            var fib = clazz.DefineMethod("fib", 
                MethodFlags.Public | MethodFlags.Static,
                WaveTypeCode.TYPE_I8.AsType(), ("x", WaveTypeCode.TYPE_I8));

            var fibGen = fib.GetGenerator();
            var label_if_1 = fibGen.DefineLabel();
            var label_if_2 = fibGen.DefineLabel();
            var for_1 = fibGen.DefineLabel();
            var for_body = fibGen.DefineLabel();
            
            // if (x == 0) return 0;
            fibGen.Emit(OpCodes.LDARG_0);
            fibGen.Emit(OpCodes.JMP_T, label_if_1);
            fibGen.Emit(OpCodes.LDC_I8_0);
            fibGen.Emit(OpCodes.RET);
            fibGen.UseLabel(label_if_1);
            // if (x == 1) return 1;
            fibGen.Emit(OpCodes.LDARG_0);
            fibGen.Emit(OpCodes.LDC_I8_1);
            fibGen.Emit(OpCodes.JMP_NN, label_if_2);
            fibGen.Emit(OpCodes.LDC_I8_1);
            fibGen.Emit(OpCodes.RET);
            fibGen.UseLabel(label_if_2);
            // var first, second, nth, i = 0;
            fibGen.Emit(OpCodes.LOC_INIT, new[]
            {
                WaveTypeCode.TYPE_I8, WaveTypeCode.TYPE_I8, 
                WaveTypeCode.TYPE_I8, WaveTypeCode.TYPE_I8
            });
            // second, nth = 1; i = 2;
            fibGen.Emit(OpCodes.LDC_I8_1); fibGen.Emit(OpCodes.STLOC_1);
            fibGen.Emit(OpCodes.LDC_I8_1); fibGen.Emit(OpCodes.STLOC_2);
            fibGen.Emit(OpCodes.LDC_I8_2); fibGen.Emit(OpCodes.STLOC_3);
            
            // for
            // 
            fibGen.Emit(OpCodes.JMP, for_1);
            fibGen.UseLabel(for_body);
            fibGen.Emit(OpCodes.LDLOC_0);
            fibGen.Emit(OpCodes.LDLOC_1);
            fibGen.Emit(OpCodes.ADD);
            fibGen.Emit(OpCodes.STLOC_2);
            
            fibGen.Emit(OpCodes.LDLOC_1);
            fibGen.Emit(OpCodes.STLOC_0);
            
            fibGen.Emit(OpCodes.LDLOC_2);
            fibGen.Emit(OpCodes.STLOC_1);
            
            // i++
            fibGen.Emit(OpCodes.LDLOC_3);
            fibGen.Emit(OpCodes.LDC_I8_1);
            fibGen.Emit(OpCodes.ADD);
            fibGen.Emit(OpCodes.STLOC_3);


            // i <= n
            fibGen.UseLabel(for_1);
            fibGen.Emit(OpCodes.LDARG_0);
            fibGen.Emit(OpCodes.LDLOC_3);
            fibGen.Emit(OpCodes.JMP_LQ, for_body);
            // return nth;
            fibGen.Emit(OpCodes.LDLOC_2);
            fibGen.Emit(OpCodes.RET);
            /*
        fibGen.Emit(OpCodes.LDARG_0);
        fibGen.Emit(OpCodes.STLOC_0);
        fibGen.Emit(OpCodes.LDC_I4_S, 20);
        fibGen.Emit(OpCodes.LDLOC_0);
        fibGen.Emit(OpCodes.JMP_L, label);      // if (n < 2) 
        fibGen.Emit(OpCodes.LDC_I4_S, 228);
        fibGen.Emit(OpCodes.RET);               // return n;
        fibGen.UseLabel(label);
        fibGen.Emit(OpCodes.LDC_I4_S, 1448);
        fibGen.Emit(OpCodes.RET);
         .locals { [0]: int32 }
        fibGen.Emit(OpCodes.LOC_INIT, new[] { WaveTypeCode.TYPE_I4 });
        fibGen.Emit(OpCodes.LDARG_0);           // n from args
        fibGen.Emit(OpCodes.LDC_I4_2);          // load 2
        fibGen.Emit(OpCodes.JMP_L, label);      // if (n < 2) 
        fibGen.Emit(OpCodes.LDARG_0);           // ref 'n'
        fibGen.Emit(OpCodes.RET);               // return n;
        fibGen.UseLabel(label);                 // end if
        fibGen.Emit(OpCodes.LDARG_0);           // ref 'n'
        fibGen.Emit(OpCodes.LDC_I4_1);          // load '1'
        fibGen.Emit(OpCodes.SUB);               // n - 1
        fibGen.EmitCall(OpCodes.CALL, fib);     // call fib(n - 1)
        fibGen.Emit(OpCodes.LDARG_0);           // ref 'n'
        fibGen.Emit(OpCodes.LDC_I4_2);          // load '2'
        fibGen.Emit(OpCodes.SUB);               //  n - 2
        fibGen.EmitCall(OpCodes.CALL, fib);     // call fib(n - 2)
        fibGen.Emit(OpCodes.STLOC_0);
        fibGen.Emit(OpCodes.LDLOC_0);
        fibGen.Emit(OpCodes.ADD);               // 'fib(n - 1)' + 'fib(n - 2)'
        fibGen.Emit(OpCodes.RET);
        */
            /*
             * /* (15,17)-(15,27) main.cs 
            /* 0x00000000 02            IL_0000: ldarg.0
            /* 0x00000001 18           IL_0001: ldc.i4.2
            /* 0x00000002 2F02          IL_0002: bge.s     IL_0006

            /* (16,21)-(16,30) main.cs 
            /* 0x00000004 02            IL_0004: ldarg.0
            /* 0x00000005 2A            IL_0005: ret

            /* (17,17)-(17,34) main.cs 
            /* 0x00000006 02            IL_0006: ldarg.0
            /* 0x00000007 17            IL_0007: ldc.i4.1
            /* 0x00000008 59            IL_0008: sub
            /* 0x00000009 28????????   IL_0009: call      int32 Xuy::f(int32)
            /* (18,17)-(18,34) main.cs 
            /* 0x0000000E 02            IL_000E: ldarg.0
            /* 0x0000000F 18            IL_000F: ldc.i4.2
            /* 0x00000010 59            IL_0010: sub
            /* 0x00000011 28????????    IL_0011: call      int32 Xuy::f(int32)
            /* 0x00000016 0A            IL_0016: stloc.0
            /* (19,17)-(19,33) main.cs 
            /* 0x00000017 06            IL_0017: ldloc.0
            /* 0x00000018 58            IL_0018: add
            /* (20,17)-(20,28) main.cs 
            /* 0x00000019 2A            IL_0019: ret
             */

            var method = clazz.DefineMethod("master", MethodFlags.Public, WaveTypeCode.TYPE_VOID.AsType());
            method.Flags = MethodFlags.Public | MethodFlags.Static;
            var body = method.GetGenerator();
            
            
            
            body.Emit(OpCodes.LDC_I8_S, (long)1073741823);
            body.EmitCall(OpCodes.CALL, fib);
            body.Emit(OpCodes.DUMP_0);
            body.Emit(OpCodes.RET);
            //body.EmitCall(OpCodes.CALL);


            var body_module = module.BakeByteArray();


            var asm = new InsomniaAssembly { Name = module.Name };
            
            asm.AddSegment((".code", body_module));
            
            InsomniaAssembly.WriteToFile(asm, new DirectoryInfo(@"C:\Users\ls-mi\Desktop\"));
        }
    }
}