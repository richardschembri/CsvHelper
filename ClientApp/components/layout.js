import React, { Component } from "react"
import { connect } from "react-redux"
import { withRouter } from "react-router"
import { LOCATION_CHANGE } from "react-router-redux"
import fetch from "isomorphic-fetch"
import marked from "marked"
import highlight from "highlight.js"

import Header from "./header"
import Footer from "./footer"

function wrapInColumns(text) {
	return `<div class="columns"><div class="column">${text}</div></div>`;
}

highlight.configure({
	languages: ["cs"]
});
const renderer = new marked.Renderer();
// For some reason using `this` here when this
// is a lambda function, `this` is undefined,
// so using a normal function.
renderer.code = function (code, lang) {
	if (this.options.highlight) {
		code = this.options.highlight(code, lang) || code;
	}

	return wrapInColumns(`<pre><code class="box ${lang}">${code}</code></pre>`);
}
renderer.heading = (text, level) => `<h${level} class="title is-${level}">${text}</h${level}>`;
renderer.list = (body, ordered) => {
	return ordered
		? `<div class="content"><ol>${body}</ol></div>` :
		`<div class="content"><ul>${body}</ul></div>`;
};
renderer.paragraph = text => wrapInColumns(`<p>${text}</p>`);
marked.setOptions({
	renderer,
	highlight: (code, language, callback) => {
		//code = code.replace(/</g, "&lt;").replace(/>/g, "&gt;");
		return highlight.highlightAuto(code).value;
	}
});

class Layout extends Component {

	state = {
		content: "",
		page: ""
	}

	constructor(props) {
		super(props);

		this.loadPage(this.props.history.location);
		this.props.history.listen(this.loadPage);
	}

	loadPage = (location) => {
		var page = location.pathname.replace(/\/(.*)#?.*/, "$1");
		if (page === "") {
			page = "home";
		}

		fetch(`/pages/${page}.md`, {
			method: "get",
			credentials: "same-origin",
			headers: {
				"Content-Type": "text/plain"
			}
		}).then(response => response.text()).then(content => {
			this.setState({ content, page });
		});
	}

	render() {
		const { content, page } = this.state;

		return (
			<div className="layout">
				<a className="fork-me-on-github" href="https://github.com/joshclose/csvhelper" target="_blank">
					<img src="https://s3.amazonaws.com/github/ribbons/forkme_right_gray_6d6d6d.png" alt="Fork me on GitHub" />
				</a>
				<Header />
				<div className="container">
					<div className={page}
						dangerouslySetInnerHTML={{ __html: marked(content) }}></div>
				</div>
				<Footer />
			</div >
		)
	}

}

export default withRouter(connect()(Layout))