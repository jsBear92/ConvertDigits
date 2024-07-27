import { useEffect, useState } from 'react'
import './App.css'

function App() {
  const [numList, setNumList] = useState([])
  const [numbers, setNumbers] = useState("")
  const [decimalNumber, setDecimalNumber] = useState("")

  const handleSubmit = async (e) => {
    e.preventDefault()
    let num = {
      number: numbers
    }

    await fetch('/api/Convert', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(num),
    })
    .then(response => response.json())
    .then(data => {
      setNumList([...numList, data]) // Append the new number to the list
      setNumbers("")
    })
  }

  const handleChange = (e) => {
    const num = e.target.value
    setNumbers(num);
  }

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('/api/Convert')
      const data = await response.json()
      setNumList(data)
    }
    fetchData()
  }, [])

  return (
    <>
      <div>
        <h1>Conversion Numbers</h1>
      </div>
      <div>
        <form method="post" onSubmit={handleSubmit}>
          <label htmlFor="numerical-data">Type a number</label>
          <input type="number" id="numerical-data" name="numerical-data" value={numbers} onChange={handleChange} />
          <button type="submit">Convert</button>
        </form>
      </div>
      <div>
        <ul>
          {numList && numList.map((numObj) => (
            <li key={numObj.id}>{numObj.number} DOLLARS</li>
          ))}
        </ul>
      </div>
    </>
  )
}

export default App
