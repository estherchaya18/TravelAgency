

window.onload = function () {
    d3.select(".chart")
        .selectAll("div")
        .data(data)
        .enter()
        .append("div")
        .style("height", function (d) { return d * 10 + "px"; }).style("margin-top", "auto")
        .text(function (d) { return d; });

   
};