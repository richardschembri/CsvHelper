module CsvHelper.FSharpRecords

type Nested = {
    A: string
    B: string
}

type Container = {
    Key: string
    Nested1: Nested
    Nested2: Nested
}

let nestedRecords = [|
    {
        Key = "one"
        Nested1 = { A = "a1"; B = "b1" }
        Nested2 = { A = "a2"; B = "b2" }
    }
|]
