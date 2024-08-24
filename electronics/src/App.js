import { useState } from "react";
import "./App.css";
import logo from "./IMG/phone-laptop-logo.jpg";
import ItemForm from "./components/ItemForm";

function App() {
  const [allItems, setAllItems] = useState([
    {
      Id: "1",
      Image: "./IMG/phone-laptop-logo.jpg",
      Name: "OnePlus",
      Type: "Phone",
      Description:
        "Smart phone RAM 16Gb Storage 1TB, camera 32MP, Colour: Black",
      Price: 300,
    },
    {
      Id: "2",
      Image: "./IMG/phone-laptop-logo.jpg",
      Name: "Huawei",
      Type: "Phone",
      Description:
        "Smart phone RAM 16Gb Storage 1TB, camera 48MP, Colour: Green",
      Price: 500,
    },
    {
      Id: "3",
      Image: "/IMG/phone-laptop-logo.jpg",
      Name: "Xiaomi",
      Type: "Phone",
      Description:
        "Smart phone RAM 16Gb Storage 1TB, camera 48MP, Colour: White",
      Price: 400,
    },
    {
      Id: "4",
      Image: "~/IMG/phone-laptop-logo.jpg",
      Name: "HP",
      Type: "Laptop",
      Description: "Laptop RAM 16Gb Storage 1TB, camera 16MP, Colour: Black",
      Price: 700,
    },
    {
      Id: "5",
      Image: "./IMG/phone-laptop-logo.jpg",
      Name: "Acer",
      Type: "Laptop",
      Description: "Laptop RAM 16Gb Storage 1TB, camera 32MP, Colour: Black",
      Price: 900,
    },
  ]);

  const [showForm, setShowForm] = useState("false");

  const OpenForm = () => {
    setShowForm(true);
  };

  return (
    <div>
      {showForm ? <ItemForm setShowForm={setShowForm} /> : null}
      <div className={showForm ? "BlurContent" : ""}>
        <h2 class="Header">
          Electrical goods, computing, phones and accessories in one place
        </h2>
        <div className="AddItemBtn-Container">
          <button className="addItemBtn" onClick={OpenForm}>
            Add Item
          </button>
        </div>
        <table>
          <tr>
            <th className="ImageTable" width="200">
              Image
            </th>
            <th className="NameTable">Name</th>
            <th className="TypeTable">Type</th>
            <th className="DescriptionTable">Description</th>
            <th className="PriceTable">Price</th>
          </tr>
          {allItems?.map((el) => {
            return (
              <tr>
                <td className="Imagetable">
                  <img src={logo} alt="logo" width="200" />
                </td>
                <td className="NameTable">{el.Name}</td>
                <td className="TypeTable">{el.Type}</td>
                <td className="DescriptionTable">{el.Description}</td>
                <td className="PriceTable">£ {el.Price}</td>
                <div className="ActionBtn">
                  <button>Edit</button>
                  <button>Delete</button>
                </div>
              </tr>
            );
          })}
        </table>
      </div>
    </div>
  );
}

export default App;
