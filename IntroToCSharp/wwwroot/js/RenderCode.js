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

/**
 * Given an id to an element and source code to highlight
 * sets uses highlight.js to highlight the element.
 */
function HighlightCode(id) {
    const el = document.getElementById(id);
    const raw = document.getElementById(`${id}-raw`);
    // Blazor adds in a <!--!--> to the begining of the element 
    // so trim wasn't working properly. We remove it first then trim.
    el.innerHTML = raw.innerHTML.replace("<!--!-->", "").trim();
    hljs.highlightElement(el);
}