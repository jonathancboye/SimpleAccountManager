module Accounts

type Account = { Balance: float }

type Transaction =
    private
    | Deposit of float
    | WithDraw of float
    | Error

let public createDeposit (money: float): Transaction =
    if money > 0.0 then Deposit money else Error

let public createWithDraw (money: float): Transaction =
    if money > 0.0 then WithDraw money else Error

let public initAccount : Account = { Balance = 0.0 }

let private deposit (money: float) (account: Account): Account =
    { account with
          Balance = account.Balance + money }

let private withDraw (money: float) (account: Account): Account =
    let newBalance = account.Balance - money
    if newBalance > 0.0 then { account with Balance = newBalance } else account

let public updateAccountBalance (account: Account) (action: Transaction): Account =
    match action with
    | Deposit money -> deposit money account
    | WithDraw money -> withDraw money account
    | Error -> account