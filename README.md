# Zinc.Demos

```
dotnet build && dotnet run --project .\Zinc.Demos\Zinc.Demos.csproj
```

# web notes
- make sure all the emscripten stuff is in your path
```
Install-Emscripten.psl script (clone and install emsdk)
https://github.com/dotnet/runtimelab/blob/feature/NativeAOT-LLVM/docs/using-nativeaot/compiling.md
dotnet workload install wasm-tools (for wasm compiling)

-fno-stack-protector (build and link)
-sFORCE-GL2 (link)
```

# to build and run web
```
dotnet clean && dotnet publish -r browser-wasm -c Debug
emrun C:\Users\kylek\Workspace\zinc\repos\Zinc.Demos\Zinc.Demos\bin\Debug\net9.0\browser-wasm\publish\ZincDemos.html
```