import React, { useState, useEffect } from 'react';
import config from '../config';
import {useParams} from "react-router-dom";
import {Dropdown, DropdownItem, DropdownMenu, DropdownToggle} from "reactstrap";

export const BankAccount = () => {
    BankAccount.displayName = 'BankAccount';

    const { personId } = useParams()

    const [loading, setLoading] = useState(true);
    const [bankAccounts, setBankAccounts] = useState([]);
    const [person, setPerson] = useState([]);
    const [dropdownOpen, setDropdownOpen] = useState(false);

    useEffect( () => {
        async function populateBankAccountData() {
            try {
                const response = await fetch(`${config.apiUrl}/api/person/${personId}`);
                const data = await response.json();
                setPerson(data);
                setBankAccounts(data.accounts);
                setLoading(false);
            } catch (error) {
                console.error('Error fetching data:', error);
                setLoading(false);
            }
        }

        populateBankAccountData();
    }, []);

    const toggleDropdown = () => {
        dropdownOpen
            ? setDropdownOpen(false)
            : setDropdownOpen(true)
    }
    
    const handleItemClick = async (accountType) => {
        const postData = {
            "accountNumber": 0,
            "balance": 0,
            "accountType": accountType,
            "signUpDate": "2023-11-24T04:04:02.452Z",
            "personId": personId
        };
        
        try {
            const response = await fetch(`${config.apiUrl}/api/BankAccount`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(postData),
            });

            if (!response.ok) {
                throw new Error('call was not ok');
            }
            
            const responseData = await response.json();
            window.location.reload();
        } catch (error) {
            console.error('Error during fetch operation:', error.message);
        }
    }

    const renderPersonDetails = () => {
        const {personId, firstName, lastName} = person
        
        return (
            <div>
                <Dropdown isOpen={dropdownOpen} toggle={toggleDropdown}>
                    <DropdownToggle caret>
                        Create New Bank Account
                    </DropdownToggle>
                    <DropdownMenu>
                        <DropdownItem onClick={async () => await handleItemClick(0)}>Chequings</DropdownItem>
                        <DropdownItem onClick={async () => await handleItemClick(1)}>Savings</DropdownItem>
                        <DropdownItem onClick={async () => await handleItemClick(2)}>Credit</DropdownItem>
                    </DropdownMenu>
                    
                </Dropdown>                
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
                        <>
                            <tr key={personId}>
                                <td>{firstName}</td>
                                <td>{lastName}</td>
                            </tr>
                            {bankAccounts.map((account) => (
                                <tr key={account.bankAccountId}>
                                    <td>{account.bankAccountId}</td>
                                </tr>
                            ))}
                        </>
                        
                    )}
                    </tbody>
                </table>
            </div>
        );
    };
    
    
    

    return (
        <div>
            <h1 id="tableLabel">Bank Accounts</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {renderPersonDetails()}
        </div>
    );
};

export default BankAccount;