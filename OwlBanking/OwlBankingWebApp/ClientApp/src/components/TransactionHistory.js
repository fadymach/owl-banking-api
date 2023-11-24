import React, { useState, useEffect } from 'react';
import config from '../config';
import {useParams} from "react-router-dom";

export const TransactionHistory = () => {
    TransactionHistory.displayName = 'TransactionHistory';

    const { personId } = useParams()

    const [loading, setLoading] = useState(true);
    const [transactions, setTransactions] = useState([]);

    useEffect( () => {
        async function populateTransactionData() {
            try {
                const response = await fetch(`${config.apiUrl}/api/BankAccount/BankAccountTransactions/${personId}`);
                const data = await response.json();
                setTransactions(data);
                setLoading(false);
            } catch (error) {
                console.error('Error fetching data:', error);
                setLoading(false);
            }
        }

        populateTransactionData();
    }, []);

    const renderTransactionHistoryTable = () => {
        return (
            <table className='table table-striped' aria-labelledby="tableLabel">
                <thead>
                <tr>
                    <th>Name</th>
                </tr>
                </thead>
                <tbody>
                {loading ? (
                    <tr>
                        <td>Loading...</td>
                    </tr>
                ) : (
                    transactions.map((transaction) => (
                        <tr key={transaction.transactionId}>
                            <td>{transaction.transactionId}</td>
                        </tr>
                    ))
                )}
                </tbody>
            </table>
        );
    };
    

    return (
        <div>
            <h1 id="tableLabel">Transaction History</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {renderTransactionHistoryTable()}
        </div>
    );
};

export default TransactionHistory;