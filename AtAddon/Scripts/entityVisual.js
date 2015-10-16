﻿function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
var roomNumber = getParameterByName('roomid');
var url = '/api/report?roomid=' + roomNumber + '&type=0';
$('#room_number').text(roomNumber);
$('.room-nav').each(function () {
    var href = $(this).attr('href');

    if (href) {
        href += (href.match(/\?/) ? '&' : '?') + 'roomid=' + getParameterByName('roomid');
        $(this).attr('href', href);
    }
});
$.get(url, function (data) {

    if (data == '{"status":"UNKNOWN_ERROR"}') {
        $("#waiting").hide();
        $("#entity-main").html('<h3 style="text-align:center">Room is not available for entity visualization to<br> your credential.</h3>');
        return;
    }
    if (data.data.entities.length == 0)
    {
        $("#waiting").hide();
        $("#entity-main").html('<h3 style="text-align:center">Insuffecient data available for a sensible entity visualization.</h3>');
        return;

    }
    var addCatToRoot = function (cat) {
        if (findCat(cat.name) == -1) {
            var size = pres.children.push(cat);
            pres.available[cat.name] = 1;
            return size - 1;
        }
        else {
            return findCat(cat.name);
        }
    }
    var findCat = function (name) {
        if (name in pres.available) {
            for (var i = 0; i < pres.children.length; i++) {
                if (pres.children[i].name == name) {
                    return i;
                }
            }
        }
        else {
            return -1;
        }
    }
    var addWordToRoot = function (en) {
        var catag = new catagory(en.type);
        var idx = addCatToRoot(catag);
        pres.children[idx].children.push(new word(en.text, en.count));
    }
    var rootnode = function () {
        this.name = "root";
        this.children = [];
        this.available = [];
        this.addCat = addCatToRoot;
        this.addWord = addWordToRoot;
        this.findCat = findCat;
    }

    var catagory = function (n) {
        this.name = n;
        this.children = [];
    }

    var word = function (n, s) {
        this.name = n;
        this.size = s;
    }

    //var placeholderEntities = eval({ "status": "OK", "time_stamp": { "Value": "2015-09-23T15:05:35.000Z", "Type": 0 }, "data": { "status": "OK", "usage": "By accessing AlchemyAPI or using information generated by AlchemyAPI, you are agreeing to be bound by the AlchemyAPI Terms of Use: http://www.alchemyapi.com/company/terms.html", "url": "https://hipchatimpulse.azurewebsites.net/api/room?name=1920655&forAlchemi=true", "language": "english", "entities": [{ "type": "Company", "relevance": "0.814193", "count": "11", "text": "Google", "disambiguated": { "subType": ["AcademicInstitution", "AwardPresentingOrganization", "OperatingSystemDeveloper", "ProgrammingLanguageDeveloper", "SoftwareDeveloper", "VentureFundedCompany"], "name": "Google", "website": "http://www.google.com/", "dbpedia": "http://dbpedia.org/resource/Google", "freebase": "http://rdf.freebase.com/ns/m.045c7b", "yago": "http://yago-knowledge.org/resource/Google", "crunchbase": "http://www.crunchbase.com/company/google" } }, { "type": "FieldTerminology", "relevance": "0.746447", "count": "7", "text": "app engine" }, { "type": "City", "relevance": "0.527971", "count": "7", "text": "Hipchat" }, { "type": "Organization", "relevance": "0.461998", "count": "3", "text": "SNI" }, { "type": "City", "relevance": "0.393771", "count": "3", "text": "stockholm" }, { "type": "FieldTerminology", "relevance": "0.386005", "count": "2", "text": "meta data" }, { "type": "Person", "relevance": "0.373582", "count": "2", "text": "Appengine" }, { "type": "FieldTerminology", "relevance": "0.365372", "count": "1", "text": "app engine." }, { "type": "OperatingSystem", "relevance": "0.364732", "count": "3", "text": "VMs" }, { "type": "Company", "relevance": "0.358747", "count": "1", "text": "Paypal", "disambiguated": { "subType": ["VentureFundedCompany"], "name": "PayPal", "dbpedia": "http://dbpedia.org/resource/PayPal", "freebase": "http://rdf.freebase.com/ns/m.01btsf", "yago": "http://yago-knowledge.org/resource/PayPal", "crunchbase": "http://www.crunchbase.com/company/paypal" } }, { "type": "Technology", "relevance": "0.33888", "count": "1", "text": "IRC" }, { "type": "FieldTerminology", "relevance": "0.301006", "count": "1", "text": "instant messaging" }, { "type": "FieldTerminology", "relevance": "0.300083", "count": "1", "text": "chat room" }, { "type": "Person", "relevance": "0.298485", "count": "1", "text": "Jerub" }, { "type": "OperatingSystem", "relevance": "0.296702", "count": "1", "text": "linux" }, { "type": "Person", "relevance": "0.294764", "count": "1", "text": "kjvenky" }, { "type": "Company", "relevance": "0.293791", "count": "1", "text": "YouTube", "disambiguated": { "subType": ["Website", "BroadcastDistributor", "FilmDistributor", "VentureFundedCompany", "AwardWinner"], "name": "YouTube", "website": "http://www.youtube.com/", "dbpedia": "http://dbpedia.org/resource/YouTube", "freebase": "http://rdf.freebase.com/ns/m.09jcvs", "yago": "http://yago-knowledge.org/resource/YouTube", "crunchbase": "http://www.crunchbase.com/company/youtube" } }, { "type": "Company", "relevance": "0.288104", "count": "1", "text": "HipGif" }, { "type": "Technology", "relevance": "0.28768", "count": "1", "text": "ssl" }, { "type": "Person", "relevance": "0.286466", "count": "1", "text": "Strom" }, { "type": "OperatingSystem", "relevance": "0.277226", "count": "1", "text": "Ubuntu" }, { "type": "FieldTerminology", "relevance": "0.264625", "count": "1", "text": "shut down" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "2", "text": "@HipChat" }, { "type": "Hashtag", "relevance": "0.264625", "count": "2", "text": "#gcloud" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "2", "text": "@" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "1", "text": "@GourabNag" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "1", "text": "@kichuku" }, { "type": "Quantity", "relevance": "0.264625", "count": "1", "text": "4 hours" }, { "type": "Quantity", "relevance": "0.264625", "count": "1", "text": "one day" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "1", "text": "@Jerub" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "1", "text": "@Strom" }, { "type": "Quantity", "relevance": "0.264625", "count": "1", "text": "1 day" }, { "type": "TwitterHandle", "relevance": "0.264625", "count": "1", "text": "@DNag" }, { "type": "Quantity", "relevance": "0.264625", "count": "1", "text": "$20" }, { "type": "Quantity", "relevance": "0.264625", "count": "1", "text": "1MB" }] } });
    var entities = data.data.entities;
    var pres = new rootnode();
    var entityTypeFreq = [];
    var entityFreq = [];
    for (var i = 0; i < entities.length; i++)
    {
        if(entities[i].type in entityTypeFreq)
        {
            entityTypeFreq[entities[i].type] += parseInt( entities[i].count, 10);
        }
        else
        {
            entityTypeFreq[entities[i].type] = parseInt(entities[i].count, 10);
        }
        entityFreq[entities[i].text] = parseInt(entities[i].count, 10);
    }
    var eFreq = [];
    var totETypeCount = 0;
    for (var key in entityTypeFreq) {
        if (entityTypeFreq.hasOwnProperty(key)) {
            eFreq.push({ type: key, count: entityTypeFreq[key] });
            totETypeCount += entityTypeFreq[key];
            $("select#cat1-drop").append($("<option>")
                .val(key)
                .html(key)
            );
            $("select#cat2-drop").append($("<option>")
                .val(key)
                .html(key)
            );
            
        }
    }
    ops = $("#cat1-drop").children();
    if (ops.length < 2)
    {
        $('#cat2-drop').val(ops[0].value)
    }
    {
        $('#cat2-drop').val(ops[1].value);

    }



    var heatUrl = heatMapUrlBase + encodeURIComponent(ops[0].value);
    $.get(heatUrl, function (data) {
        chk = data;
        var tempLab = [];
        var counts = [];
        var leb = [];
        for (var key in data.Result) {
            console.log(new Date(key));
            var cDate = new Date(key);
            tempLab.push(cDate);
            counts.push(data.Result[key]);
            leb.push(cDate.getMonth() + "/" + cDate.getDate());
        }
        Linedata.labels = leb;
        Linedata.datasets[0].data = counts;
        var myLineChart = new Chart(ctx).Line(Linedata, options);
    });
    var heatUrl = heatMapUrlBase + encodeURIComponent(ops[1].value);
    $.get(heatUrl, function (data) {
        chk = data;
        var tempLab = [];
        var counts = [];
        var leb = [];
        for (var key in data.Result) {
            console.log(new Date(key));
            var cDate = new Date(key);
            tempLab.push(cDate);
            counts.push(data.Result[key]);
            leb.push(cDate.getMonth() + "/" + cDate.getDate());
        }
        Linedata.labels = leb;
        Linedata.datasets[1].data = counts;
        var myLineChart = new Chart(ctx).Line(Linedata, options);
    });





    eFreq.sort(function (a, b) { return b.count - a.count; });
    $("#hottest-en-count").text(eFreq[0].type);
    $("#least-en-count").text(eFreq[eFreq.length - 1].type);
    for (var i = 0; i < eFreq.length && i< 5; i++)
    {
        var temp = (eFreq[i].count * 100) / totETypeCount;
        var htmlStr = '<tr><td>' + (i+1) + '.</td><td>' +
            eFreq[i].type + '</td><td><span class="badge bg-aqua">' + Math.round(temp * 100) / 100
            + '%</span></td></tr>';
        var newEType = $(htmlStr).hide();
        $('#etype-table').append(newEType);
        newEType.show('normal');
    }
    var eFreqWord = [];
    var totECount = 0;
    for (var key in entityFreq) {
        if (entityFreq.hasOwnProperty(key)) {
            eFreqWord.push({ word: key, count: entityFreq[key] });
            totECount += entityFreq[key];
            
        }
    }
    eFreqWord.sort(function (p, q) { return q.count - p.count; });
    $("#waiting").hide();
    for (var i = 0; i < entities.length; i++) {
        pres.addWord(entities[i]);
    }
    $("#hottest-en").text(eFreqWord[0].word);
    $("#least-en").text(eFreqWord[eFreqWord.length - 1].word);
    for (var i = 0; i < eFreqWord.length && i < 5; i++)
    {
        var temp = (eFreqWord[i].count * 100) / totETypeCount;
        var htmlStr = '<tr><td>' + (i + 1) + '.</td><td>' +
            eFreqWord[i].word + '</td><td><span class="badge bg-aqua">' + Math.round(temp * 100) / 100
            + '%</span></td></tr>';
        var newE = $(htmlStr).hide();
        $('#e-table').append(newE);
        newE.show('normal');
    }
    // worker starts
    // Dimensions of sunburst.
    var width = 600;
    var height = 500;
    var radius = Math.min(width, height) / 2;

    // Breadcrumb dimensions: width, height, spacing, width of tip/tail.
    var b = {
        w: 75, h: 30, s: 3, t: 10
    };

    // make `colors` an ordinal scale
    var colors = d3.scale.category20c();

    // Total size of all segments; we set this later, after loading the data.
    var totalSize = 0;

    var vis = d3.select("#entity-chart").append("svg:svg")
        .attr("width", width)
        .attr("height", height)
        .append("svg:g")
        .attr("id", "container")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var partition = d3.layout.partition()
        .size([2 * Math.PI, radius * radius])
        .value(function (d) { return d.size; });

    var arc = d3.svg.arc()
        .startAngle(function (d) { return d.x; })
        .endAngle(function (d) { return d.x + d.dx; })
        .innerRadius(function (d) { return Math.sqrt(d.y); })
        .outerRadius(function (d) { return Math.sqrt(d.y + d.dy); });

    // Use d3.csv.parseRows so that we do not need to have a header
    // row, and can receive the csv as an array of arrays.

    //var text = getText();
    //var csv = d3.csv.parseRows(text);
    //var json = buildHierarchy(csv);
    var json = getData();
    createVisualization(json);

    // Main function to draw and set up the visualization, once we have the data.
    function createVisualization(json) {

        // Basic setup of page elements.
        initializeBreadcrumbTrail();

        d3.select("#entity-togglelegend").on("click", toggleLegend);

        // Bounding circle underneath the sunburst, to make it easier to detect
        // when the mouse leaves the parent g.
        vis.append("svg:circle")
            .attr("r", radius)
            .style("opacity", 0);

        // For efficiency, filter nodes to keep only those large enough to see.
        var nodes = partition.nodes(json)
            .filter(function (d) {
                return (d.dx > 0.005); // 0.005 radians = 0.29 degrees
            });

        var uniqueNames = (function (a) {
            var output = [];
            a.forEach(function (d) {
                if (output.indexOf(d.name) === -1) {
                    output.push(d.name);
                }
            });
            return output;
        })(nodes);

        // set domain of colors scale based on data
        colors.domain(uniqueNames);

        // make sure this is done after setting the domain
        drawLegend();


        var path = vis.data([json]).selectAll("path")
            .data(nodes)
            .enter().append("svg:path")
            .attr("display", function (d) { return d.depth ? null : "none"; })
            .attr("d", arc)
            .attr("fill-rule", "evenodd")
            .style("fill", function (d) { return colors(d.name); })
            .style("opacity", 1)
            .on("mouseover", mouseover);

        // Add the mouseleave handler to the bounding circle.
        d3.select("#container").on("mouseleave", mouseleave);

        // Get total size of the tree = value of root node from partition.
        totalSize = path.node().__data__.value;
    };

    // Fade all but the current sequence, and show it in the breadcrumb trail.
    function mouseover(d) {

        //var percentage = (100 * d.value / totalSize).toPrecision(3);
        var percentage = d.value;
        //var percentageString = percentage + "%";
        var percentageString = percentage + " Times";
        //if (percentage < 0.1) {
        //  percentageString = "< 0.1%";
        //}

        d3.select("#entity-percentage")
            .text(percentageString);

        d3.select("#explanation")
            .style("visibility", "");

        var sequenceArray = getAncestors(d);
        updateBreadcrumbs(sequenceArray, percentageString);

        // Fade all the segments.
        d3.selectAll("path")
            .style("opacity", 0.3);

        // Then highlight only those that are an ancestor of the current segment.
        vis.selectAll("path")
            .filter(function (node) {
                return (sequenceArray.indexOf(node) >= 0);
            })
            .style("opacity", 1);
    }

    // Restore everything to full opacity when moving off the visualization.
    function mouseleave(d) {

        // Hide the breadcrumb trail
        d3.select("#trail")
            .style("visibility", "hidden");

        // Deactivate all segments during transition.
        d3.selectAll("path").on("mouseover", null);

        // Transition each segment to full opacity and then reactivate it.
        d3.selectAll("path")
            .transition()
            .duration(1000)
            .style("opacity", 1)
            .each("end", function () {
                d3.select(this).on("mouseover", mouseover);
            });

        d3.select("#explanation")
            .transition()
            .duration(1000)
            .style("visibility", "hidden");
    }

    // Given a node in a partition layout, return an array of all of its ancestor
    // nodes, highest first, but excluding the root.
    function getAncestors(node) {
        var path = [];
        var current = node;
        while (current.parent) {
            path.unshift(current);
            current = current.parent;
        }
        return path;
    }

    function initializeBreadcrumbTrail() {
        // Add the svg area.
        var trail = d3.select("#entity-sequence").append("svg:svg")
            .attr("width", width)
            .attr("height", 50)
            .attr("id", "trail");
        // Add the label at the end, for the percentage.
        trail.append("svg:text")
          .attr("id", "endlabel")
          .style("fill", "#000");
    }

    // Generate a string that describes the points of a breadcrumb polygon.
    function breadcrumbPoints(d, i) {
        var points = [];
        points.push("0,0");
        points.push((b.w * 2) + ",0");
        points.push(((b.w * 2) + b.t) + "," + (b.h / 2));
        points.push((b.w * 2) + "," + b.h);
        points.push("0," + b.h);
        if (i > 0) { // Leftmost breadcrumb; don't include 6th vertex.
            points.push(b.t + "," + (b.h / 2));
        }
        return points.join(" ");
    }

    // Update the breadcrumb trail to show the current sequence and percentage.
    function updateBreadcrumbs(nodeArray, percentageString) {

        // Data join; key function combines name and depth (= position in sequence).
        var g = d3.select("#trail")
            .selectAll("g")
            .data(nodeArray, function (d) { return d.name + d.depth; });

        // Add breadcrumb and label for entering nodes.
        var entering = g.enter().append("svg:g");

        entering.append("svg:polygon")
            .attr("points", breadcrumbPoints)
            .style("fill", function (d) { return colors(d.name); });

        entering.append("svg:text")
            .attr("x", ((b.w * 2) + b.t) / 2)
            .attr("y", b.h / 2)
            .attr("dy", "0.35em")
            .attr("text-anchor", "middle")
            .text(function (d) { return d.name; });

        // Set position for entering and updating nodes.
        g.attr("transform", function (d, i) {
            return "translate(" + i * ((b.w * 2) + b.s) + ", 0)";
        });

        // Remove exiting nodes.
        g.exit().remove();

        // Now move and update the percentage at the end.
        d3.select("#trail").select("#endlabel")
            .attr("x", (nodeArray.length + 0.5) * ((b.w * 2) + b.s))
            .attr("y", b.h / 2)
            .attr("dy", "0.35em")
            .attr("text-anchor", "middle")
            .text(percentageString);

        // Make the breadcrumb trail visible, if it's hidden.
        d3.select("#trail")
            .style("visibility", "");

    }

    function drawLegend() {

        // Dimensions of legend item: width, height, spacing, radius of rounded rect.
        var li = {
            w: 75, h: 30, s: 3, r: 3
        };

        var legend = d3.select("#entity-legend").append("svg:svg")
            .attr("width", li.w)
            .attr("height", colors.domain().length * (li.h + li.s));

        var g = legend.selectAll("g")
            .data(colors.domain())
            .enter().append("svg:g")
            .attr("transform", function (d, i) {
                return "translate(0," + i * (li.h + li.s) + ")";
            });

        g.append("svg:rect")
            .attr("rx", li.r)
            .attr("ry", li.r)
            .attr("width", li.w)
            .attr("height", li.h)
            .style("fill", function (d) { return colors(d); });

        g.append("svg:text")
            .attr("x", li.w / 2)
            .attr("y", li.h / 2)
            .attr("dy", "0.35em")
            .attr("text-anchor", "middle")
            .text(function (d) { return d; });
    }

    function toggleLegend() {
        var legend = d3.select("#entity-legend");
        if (legend.style("visibility") == "hidden") {
            legend.style("visibility", "");
        } else {
            legend.style("visibility", "hidden");
        }
    }

    // Take a 2-column CSV and transform it into a hierarchical structure suitable
    // for a partition layout. The first column is a sequence of step names, from
    // root to leaf, separated by hyphens. The second column is a count of how 
    // often that sequence occurred.
    function buildHierarchy(csv) {
        var root = { "name": "root", "children": [] };
        for (var i = 0; i < csv.length; i++) {
            var sequence = csv[i][0];
            var size = +csv[i][1];
            if (isNaN(size)) { // e.g. if this is a header row
                continue;
            }
            var parts = sequence.split("-");
            var currentNode = root;
            for (var j = 0; j < parts.length; j++) {
                var children = currentNode["children"];
                var nodeName = parts[j];
                var childNode;
                if (j + 1 < parts.length) {
                    // Not yet at the end of the sequence; move down the tree.
                    var foundChild = false;
                    for (var k = 0; k < children.length; k++) {
                        if (children[k]["name"] == nodeName) {
                            childNode = children[k];
                            foundChild = true;
                            break;
                        }
                    }
                    // If we don't already have a child node for this branch, create it.
                    if (!foundChild) {
                        childNode = { "name": nodeName, "children": [] };
                        children.push(childNode);
                    }
                    currentNode = childNode;
                } else {
                    // Reached the end of the sequence; create a leaf node.
                    childNode = { "name": nodeName, "size": size };
                    children.push(childNode);
                }
            }
        }
        return root;
    };

    function getData() {
        return pres;
    };


});




var ctx = $("#com-chart").get(0).getContext("2d");
var Linedata = {
    labels: [],
    datasets: [

        {
            label: "My First dataset",
            fillColor: "rgba(220,220,220,0.2)",
            strokeColor: "rgba(220,220,220,1)",
            pointColor: "rgba(220,220,220,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(220,220,220,1)",
            data: []
        },
        {
            label: "My Second dataset",
            fillColor: "rgba(151,187,205,0.2)",
            strokeColor: "rgba(151,187,205,1)",
            pointColor: "rgba(151,187,205,1)",
            pointStrokeColor: "#fff",
            pointHighlightFill: "#fff",
            pointHighlightStroke: "rgba(151,187,205,1)",
            data: []
        }
    ]
};
var options = {

    ///Boolean - Whether grid lines are shown across the chart
    scaleShowGridLines: true,

    //String - Colour of the grid lines
    scaleGridLineColor: "rgba(0,0,0,.05)",

    //Number - Width of the grid lines
    scaleGridLineWidth: 1,

    //Boolean - Whether to show horizontal lines (except X axis)
    scaleShowHorizontalLines: true,

    //Boolean - Whether to show vertical lines (except Y axis)
    scaleShowVerticalLines: true,

    //Boolean - Whether the line is curved between points
    bezierCurve: true,

    //Number - Tension of the bezier curve between points
    bezierCurveTension: 0.4,

    //Boolean - Whether to show a dot for each point
    pointDot: true,

    //Number - Radius of each point dot in pixels
    pointDotRadius: 4,

    //Number - Pixel width of point dot stroke
    pointDotStrokeWidth: 1,

    //Number - amount extra to add to the radius to cater for hit detection outside the drawn point
    pointHitDetectionRadius: 20,

    //Boolean - Whether to show a stroke for datasets
    datasetStroke: true,

    //Number - Pixel width of dataset stroke
    datasetStrokeWidth: 2,

    //Boolean - Whether to fill the dataset with a colour
    datasetFill: true,

    //String - A legend template
    legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

};

var chk;
var heatMapUrlBase = '/api/HeatMap?roomid=' + roomNumber + '&cat=';
$('#cat1-drop').on('change', function () {
    var heatUrl = heatMapUrlBase + encodeURIComponent(this.value);
    $.get(heatUrl, function (data) {
        chk = data;
        var tempLab = [];
        var counts = [];
        var leb = [];
        for (var key in data.Result) {
            console.log(new Date(key));
            var cDate = new Date(key);
            tempLab.push(cDate);
            counts.push(data.Result[key]);
            leb.push(cDate.getMonth() + "/" + cDate.getDate());
        }
        Linedata.labels = leb;
        Linedata.datasets[0].data = counts;
        var myLineChart = new Chart(ctx).Line(Linedata, options);
    });
    //alert(this.value); // or $(this).val()
});
$('#cat2-drop').on('change', function () {
    var heatUrl = heatMapUrlBase + encodeURIComponent(this.value);
    $.get(heatUrl, function (data) {
        chk = data;
        var tempLab = [];
        var counts = [];
        var leb = [];
        for (var key in data.Result) {
            console.log(new Date(key));
            var cDate = new Date(key);
            tempLab.push(cDate);
            counts.push(data.Result[key]);
            leb.push(cDate.getMonth() + "/" + cDate.getDate());
        }
        Linedata.labels = leb;
        Linedata.datasets[1].data = counts;
        var myLineChart = new Chart(ctx).Line(Linedata, options);
    });
    //alert(this.value); // or $(this).val()
});
