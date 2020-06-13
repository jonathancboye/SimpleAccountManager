// Learn more about F# at http://fsharp.org

open System
open Accounts

type MenuState =
   | NotDone of Account
   | Done of Account

let transactionMenu =
   ["What transaction would you like to perform: "
    ; "1) Deposit"
    ; "2) Withdraw"
    ; "3) Exit"
    ]
    |> List.map (fun s -> s + Environment.NewLine)
    |> List.reduce (+)

let amountMenu (transaction:string) =
   String.Format("Enter the amount of money you want to {0}: ", transaction)

let accountBalanaceMenu account =
   String.Format("Your account balance is: {0}", account.Balance)

let printAccountBalance = 
   accountBalanaceMenu 
   >> Console.WriteLine 
   >> ignore

let printTransaction =
   amountMenu 
   >> Console.Write 
   >> ignore

let performTransaction transName transConstructor account =
   printTransaction transName
   match Double.TryParse(Console.ReadLine()) with
   |true, n -> 
      let newAccount = 
         transConstructor n 
         |> updateAccountBalance account
      printAccountBalance newAccount
      NotDone newAccount
   |false, _ -> 
      NotDone account

let depositTransaction = performTransaction "Deposit" createDeposit 

let withdrawTransaction = performTransaction "Withdraw" createWithDraw

let rec menu menuState  =
   match menuState with
   |NotDone account -> 
      Console.WriteLine transactionMenu |> ignore
      match Console.ReadLine() with
      |"1" -> 
         depositTransaction account
         |> menu
      |"2" -> 
         withdrawTransaction account
         |> menu
      |"3" ->
         account
      |_ ->
         NotDone account |> menu
   |Done account ->
      account
    
[<EntryPoint>]
let main argv =
   NotDone initAccount 
   |> menu
   |> ignore
   0 // return an integer exit code