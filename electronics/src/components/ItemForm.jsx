const ItemForm = (props) => {
  const { setShowForm } = props;

  const HandleCancel = () => {
    setShowForm(false);
  };

  return (
    <div className="FormContainer">
      <label>Image</label>
      <input
        type="file"
        name="image"
        id="image"
        className="Image"
        required="required"
      ></input>

      <label>Name</label>
      <input
        type="text"
        name="name"
        id="name"
        placeholder="Insert name of the item"
        className="Name"
        required="required"
      ></input>

      <label for="type">Type</label>
      <select name="type" id="type">
        <option value=""></option>
        <option value="phone">Phone</option>
        <option value="laptop">Laptop</option>
        <option value="tablet">Tablet</option>
        <option value="watch">Smart Watch</option>
        <option value="camera">Action Camera</option>
        <option value="speaker">Wireless Bluetooth Speaker</option>
      </select>

      <label>Description</label>
      <textarea
        type="text"
        name="description"
        id="description"
        className="Description"
        placeholder="Put your description here"
        required="required"
      />

      <label>Price</label>
      <input
        type="number"
        name="price"
        id="price"
        placeholder="Insert the price"
        className="Price"
        required="required"
      ></input>

      <button>Submit</button>
      <button onClick={HandleCancel}>Cancel</button>

      {/* <label htmlFor="Price"></label>
      <input></input> */}
    </div>
  );
};

export default ItemForm;
