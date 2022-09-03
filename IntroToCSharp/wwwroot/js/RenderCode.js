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
    const code = raw.innerHTML.replace("<!--!-->", "").trim();
    const base64 = window.btoa(code);
    // Blazor adds in a <!--!--> to the begining of the element 
    // so trim wasn't working properly. We remove it first then trim.
    el.innerHTML = raw.innerHTML.replace("<!--!-->", "").trim();
    hljs.highlightElement(el);
}

function GetVSCodeLink(id) {
    const raw = document.getElementById(`${id}-raw`);
    const code = raw.innerHTML.replace("<!--!-->", "").trim();
    const base64 = window.btoa(code);
    return `vscode://captain-coder.adventures-in-c--extension/load-simple-project?program.cs=${base64}`;
}

function get_shortest_prefix(lines) {
    let shortest_prefix = Infinity;
    let ix;
    for (let line of lines) {
        if (line.trim().length == 0) continue;
        for (ix = 0; ix < line.length; ix++) {
            if (line[ix] == ' ') continue;
            break;
        }
        if (ix < shortest_prefix) {
            shortest_prefix = ix;
        }
    }
    return shortest_prefix;
}

function format_code_block(lines) {
    const shortestPrefix = get_shortest_prefix(lines);
    return lines.map(s => s.substring(shortestPrefix)).join("\n");
}

/**
 * Given an id to an <Output> element trims it and makes it look nice
 */
function RenderOutput(id) {
    const el = document.getElementById(id);
    const raw = document.getElementById(`${id}-raw`);
    // Blazor adds in a <!--!--> to the begining of the element 
    // so trim wasn't working properly. We remove it first then trim.
    const text = raw.innerHTML.replace("<!--!-->", "");
    if (!text) return;
    el.innerHTML = format_code_block(text.split("\n").slice(1));
}