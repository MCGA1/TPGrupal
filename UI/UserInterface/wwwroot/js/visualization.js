var datainterval;
var visualizationChart;
var donutChart;

var bultos_ingresados;
var bultos_enproceso;
var bultos_apilados;

$(document).ready(function () {
	setVariables();
	initializeGraph();
	initializeDatareader();
});

window.onunload = function () {
	unsetDatareader();
}

function setVariables() {
	bultos_ingresados = document.getElementById("bultos_ingresados");
	bultos_enproceso = document.getElementById("bultos_enproceso");
	bultos_apilados = document.getElementById("bultos_apilados");
}

function initializeGraph() {
	var times = [];
	for (var i = 0; i < 24; i++) {
		if (i < 10) {
			times.push("0" + i + ":00");
		} else {
			times.push(i + ":00");
		}
	}

	var ctx = document.getElementById('visualizationChart').getContext('2d');
	visualizationChart = new Chart(ctx, {
		type: 'line',
		data: {
			labels: times,
			datasets: [{
				label: 'Ingresados',
				//data: [12, 19, 3, 5, 2, 3, 4, 12, 20, 23, 3, 4, 21, 52, 53, 11, 2, 13, 17, 8, 21, 12, 44, 16],
				backgroundColor: [
					'rgba(255, 206, 86, 0.2)'
				],
				borderColor: [
					'rgba(255, 206, 86, 1)'
				],
				borderWidth: 1
			},
			{
				label: 'Apilados',
				//data: [2, 10, 5, 7, 1, 0, 2, 16, 14, 19, 5, 0, 10, 60, 40, 3, 8, 10, 21, 2, 19, 15, 30, 4],
				backgroundColor: [
					'rgba(54, 162, 235, 0.2)'
				],
				borderColor: [
					'rgba(54, 162, 235, 1)'
				],
				borderWidth: 1
			}
			]
		},
		options: {
			scales: {
				y: {
					beginAtZero: true,
				},
				xAxes: [{
					display: true,
					gridLines: {
						display: false
					},
					scaleLabel: {
						display: true,
						labelString: 'Hora',
						fontColor: "#C8D8E4"
					},
					ticks: {
						fontColor: "#C8D8E4",
					},
				}],
				yAxes: [{
					display: true,
					gridLines: {
						display: false
					},
					scaleLabel: {
						display: true,
						labelString: 'Cantidad',
						fontColor: "#C8D8E4"
					},
					ticks: {
						fontColor: "#C8D8E4",
					},
				}]
			},
			responsive: true,
			title: {
				display: true,
				fontColor: "#C8D8E4",
				text: 'Ingresados/Procesados'
			},
			legend: {
				labels: {
					fontColor: '#C8D8E4'
				}
			}
		}
	});

	var donut_ctx = document.getElementById('visualizationDonut').getContext('2d');
	donutChart = new Chart(donut_ctx, {
		type: 'doughnut',
		data: {
			labels: ['En proceso', 'Apilados'],
			datasets: [
				{
					label: 'Grafico de bultos',
					//data: [13, 8],
					backgroundColor: [
						'rgba(255, 99, 132, 0.2)',
						'rgba(54, 162, 235, 0.2)'

					],
					borderColor: [
						'rgba(255, 99, 132, 1)',
						'rgba(54, 162, 235, 1)'
					],
					borderWidth: 1
				}
			]
		},
		options: {
			responsive: true,
			title: {
				display: true,
				fontColor: "#C8D8E4",
				text: 'Grafico de bultos en proceso/apilados'
			},
			legend: {
				labels: {
					fontColor: '#C8D8E4'
				}
			}
		},
	});

}

function initializeDatareader() {
	readDataFromServer();
	datainterval = setInterval(readDataFromServer, 10000);
}

function readDataFromServer() {
	$.ajax({
		url: '/Visualization/ReadData',
		method: 'GET'
	}).then(function (data) {
		console.log(data);

		bultos_ingresados.textContent = data.ingresados.length;
		var arrayIngresados = getArrayForData(data.ingresados);
		bultos_enproceso.textContent = data.enproceso.length;
		//var arrayEnProcessos = getArrayForData(data.enproceso);
		bultos_apilados.textContent = data.apilados.length;
		var arrayApilados = getArrayForData(data.apilados);

		visualizationChart.data.datasets[0].data = arrayIngresados;
		visualizationChart.data.datasets[1].data = arrayApilados;
		visualizationChart.update();

		donutChart.data.datasets[0].data = [data.enproceso.length, data.apilados.length];
		donutChart.update();

	});
}

function getArrayForData(data){
	var array = Array(24).fill(0);

	for (const item of data) {
		var index = new Date(item.creationDate).getHours() - 1;
		array[index] = array[index] + 1;
	};

	return array;
}

function unsetDatareader() {
	clearInterval(datainterval);
}
