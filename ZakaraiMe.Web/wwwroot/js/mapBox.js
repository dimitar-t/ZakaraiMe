var mapOptions = {
    zoom: 9,
    lat: $("#startPointX").val(),
    lng: $("#startPointY").val()
};

(function (context) {
    if (context.lat == 0 || context.lng == 0) {
        context.lat = 25.5;
        context.lng = 42.75;
        context.zoom = 6;
    }

    mapboxgl.accessToken = 'pk.eyJ1IjoiZHVuZHVybHVua2EiLCJhIjoiY2pzcWd6OGR6MTQ3NTQ5cjJmMDd5eTJmbyJ9.TXbTIkJ-3Sp4t7kvj94idA';

    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v9',
        center: [context.lat, context.lng],
        zoom: context.zoom
    });
    
    var mapboxDirectionsControl = new MapboxDirections({
        accessToken: mapboxgl.accessToken,
        unit: 'metric',
        controls: {
            instructions: false
        }
    });

    map.addControl(mapboxDirectionsControl, 'top-left');

    context.getMapboxDirections = function () {
        return mapboxDirectionsControl;
    };

    context.getMap = function () {
        return map;
    };
})(mapOptions);

