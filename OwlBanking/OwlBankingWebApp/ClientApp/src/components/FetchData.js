import React, { useState, useEffect } from 'react';
import config from '../config';
import { useNavigate } from 'react-router-dom';

export const FetchData = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [persons, setPersons] = useState([]);

  useEffect(() => {
      async function populatePersonData() {
          try {
              const response = await fetch(`${config.apiUrl}/Persons`);
              const data = await response.json();
              setPersons(data);
              setLoading(false);
          } catch (error) {
              console.error('Error fetching data:', error);
              setLoading(false);
          }
      }

      populatePersonData();
  }, []);

  const renderPersonTable = () => {
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
              persons.map((person) => (
                  <tr key={person.firstName}>
                    <td>{person.firstName}</td>
                    <td>
                      <button onClick={() => retrieveTransactionHistory(person.personId)}>
                        Retrieve Transaction History
                      </button>
                    </td>
                  <td>
                      <button onClick={() => createBankAccount(person.personId)}>
                          Create New Bank Account
                      </button>
                  </td>
                  </tr>
              ))
          )}
          </tbody>
        </table>
    );
  };

  const retrieveTransactionHistory = (personId) => {
    navigate(`/transaction-history/${personId}`);
  };

    const createBankAccount = (personId) => {
        navigate(`/bank-account/${personId}`);
    };

  return (
      <div>
        <h1 id="tableLabel">Persons</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {renderPersonTable()}
      </div>
  );
};