import { useEffect, useState } from "react";
import "./App.css";

function App() {
  const [numList, setNumList] = useState([]);
  const [numbers, setNumbers] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    let numObj = {};

    if (numbers.includes(".")) {
      numObj = {
        dollars: numbers.split(".")[0],
        cents: numbers.split(".")[1],
      };
    } else {
      numObj = {
        dollars: numbers,
        cents: "",
      };
    }
    if (numObj.cents.length === 1) numObj.cents = numObj.cents + "0";

    console.log(numObj);

    await fetch("/api/Convert", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(numObj),
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error(response);
        }
      })
      .then((data) => {
        setNumList([...numList, data]);
        setNumbers("");
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  };

  const handleChange = (e) => {
    const num = e.target.value;
    setNumbers(num);
  };

  const handlePlural = (num) => {
    const dollarWord = num != "ONE" ? " DOLLARS" : " DOLLAR";
    return num + dollarWord;
  }

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch("/api/Convert");
      const data = await response.json();
      setNumList(data);
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
            type="number"
            id="numerical-data"
            name="numerical-data"
            value={numbers}
            onChange={handleChange}
          />
          <button type="submit">Convert</button>
        </form>
      </div>
      <div>
        <ul>
          {numList &&
            numList.map((numObj) => (
              <li key={numObj.id}>
                {!numObj.cents
                  ? handlePlural(numObj.dollars)
                  : handlePlural(numObj.dollars) + " AND " + numObj.cents + " CENTS"}
              </li>
            ))}
        </ul>
      </div>
    </>
  );
}

export default App;
