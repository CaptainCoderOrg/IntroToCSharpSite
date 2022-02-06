/**
 * Given an id to an element and source code to highlight
 * sets the contents of the element associated with the id
 * and uses highlight.js to highlight the element.  
 */
function RenderCode(id, content) {
    // console.log(id, content);
    let el = document.getElementById(id);
    el.innerHTML = content;
    hljs.highlightElement(el);
}