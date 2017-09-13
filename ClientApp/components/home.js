import React, { Component } from "react"
import { connect } from "react-redux"

class Home extends Component {

	render() {
		return (
			<div className="home">
				<section className="hero">
					<div className="hero-body">
						<div className="container">
							<h1>CsvHelper</h1>
						</div>
					</div>
				</section>
			</div>
		)
	}

}

export default connect()(Home)