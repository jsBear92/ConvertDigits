import { useEffect, useState } from "react";
import "./App.css";

function App() {
  const [numList, setNumList] = useState([]); // List of converted numbers
  const [numbers, setNumbers] = useState(""); // Input string number(dollars and cents)
  const [error, setError] = useState(""); // Show error message

  // Handle form submit (POST)
  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent page reload
    let numObj = {}; // Temporary object to store dollars and cents

    // Divide the input string into dollars and cents by "." decimal point
    if (numbers.includes(".")) {
      numObj = {
        dollars: numbers.split(".")[0],
        cents: numbers.split(".")[1],
      };
    } else {
      // If there is no decimal point, set cents to empty string
      numObj = {
        dollars: numbers,
        cents: "",
      };
    }

    if (numObj.cents.length === 1) numObj.cents = numObj.cents + "0"; // Add zero to cents if only one digit 1.4 -> 1.40

    // Validate numbers less than a billion or with two decimal places
    const regex = /^\d{1,9}(\.\d{1,2})?$/;
    if (!regex.test(numbers)) {
      setError(
        "Please enter a valid number less than a billion. or with two decimal places."
      );
      return;
    }

    console.log(numObj);

    // POST request to convert the number
    try {
      const response = await fetch("/api/Convert", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(numObj),
      });

      if (!response.ok) {
        throw new Error(`Error: ${response.status} ${response.statusText}`);
      }

      const data = await response.json();
      setNumList([...numList, data]); // Add the converted number to the list
      setNumbers(""); // Clear the input field
      setError(""); // Clear the error message
    } catch (error) {
      console.error("Error:", error);
      setError("An error occurred while converting the number.");
    }
  };

  // Handle input change for storing the input value
  const handleChange = (e) => {
    const num = e.target.value; // Get the input from the input field

    // Check if input is not a number
    if (isNaN(num)) {
      setError("Please enter a valid number.");
    } else {
      setError("");
    }

    setNumbers(num);
  };

  // Handle dollar words based on the number (One or more dollars)
  const handlePlural = (num) => {
    const dollarWord = num !== "ONE" ? " DOLLARS" : " DOLLAR";
    return num + dollarWord;
  };

  // Fetch the converted numbers from the server
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch("/api/Convert");
        if (!response.ok) {
          throw new Error(`Error: ${response.status} ${response.statusText}`);
        }
        const data = await response.json();
        setNumList(Array.isArray(data) ? data : []); // Ensure data is an array
      } catch (error) {
        console.error("Error fetching data:", error);
        setError("An error occurred while fetching the converted numbers.");
      }
    };
    fetchData();
  }, []);

  return (
    <>
      <div>
        <h1>Conversion Numbers</h1>
      </div>
      <div>
        <form method="post" onSubmit={handleSubmit}>
          <label htmlFor="numerical-data">Type a number</label>
          <input
            type="text"
            id="numerical-data"
            name="numerical-data"
            value={numbers}
            onChange={handleChange}
          />
          <button type="submit">Convert</button>
          {error && <p className="error">{error}</p>}{" "}
          {/* Show error msessage when it has */}
        </form>
      </div>
      <div>
        <ul>
          {Array.isArray(numList) && numList.map((result) => (
            <li key={result.id}>
              {!result.cents
                ? handlePlural(result.dollars)
                : handlePlural(result.dollars) +
                  " AND " +
                  result.cents +
                  " CENTS"}
            </li>
          ))}
        </ul>
      </div>
    </>
  );
}

export default App;