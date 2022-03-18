import star from './star.png';
import React from 'react';
import './App.css';
import { scryRenderedComponentsWithType } from 'react-dom/test-utils';

class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      reviewText: "",
      starRating: 0
    }
  }
  async handleGenerateButtonClick(){
    fetch('http://localhost:8139/API/generate')
    .then(response => response.json())
    .then(data => this.setState({
      reviewText: data.reviewText,
      starRating: data.starRating
    }));
  }
  render(){
    var stars = [];
    for(var i = 0; i < this.state.starRating; i++){
      stars.push(<img src={star}/>);
    }
    return (
      <div className="App">
        <header className="App-header">
          <button onClick={() => this.handleGenerateButtonClick()}>
            Generate Review
          </button>
          <span className="Stars">{stars}</span>
          <p className="Review">{this.state.reviewText}</p>
        </header>
      </div>
    );
  }
}

export default App;
