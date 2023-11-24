import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { TransactionHistory } from "./components/TransactionHistory";
import { BankAccount } from "./components/BankAccount";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/transaction-history/:personId',
    element: <TransactionHistory />,
  },
  {
    path: 'bank-account/:personId',
    element: <BankAccount/>
  }
];

export default AppRoutes;
