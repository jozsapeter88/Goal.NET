import { useState } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import "./DatePicker.css";

function DatePicker() {
  const [date, setDate] = useState(new Date());

  return (
    <div className="app">
      <div className="calendar-container">
        <Calendar onChange={setDate} value={date} />
      </div>
      <p>
        <span className="bold">Selected Date:</span> {date.toDateString()}
      </p>
    </div>
  );
}

export default DatePicker;
