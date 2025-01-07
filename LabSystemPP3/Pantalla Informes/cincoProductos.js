<script>
    document.addEventListener("DOMContentLoaded", function () {
        // Obtener datos desde los HiddenFields
        var labels = JSON.parse(document.getElementById('<%= labelsHiddenField.ClientID %>').value);
        var values = JSON.parse(document.getElementById('<%= valuesHiddenField.ClientID %>').value);

    // Configurar Chart.js
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'bar', // Tipo de gráfico
    data: {
        labels: labels, // Etiquetas dinámicas
    datasets: [{
        label: 'Top 5 Productos más vendidos',
    data: values, // Valores dinámicos
    backgroundColor: [
    'rgba(255, 99, 132, 0.6)',
    'rgba(54, 162, 235, 0.6)',
    'rgba(255, 206, 86, 0.6)',
    'rgba(75, 192, 192, 0.6)',
    'rgba(153, 102, 255, 0.6)'
    ],
    borderColor: [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)'
    ],
    borderWidth: 1
                }]
            },
    options: {
        responsive: true,
    title: {
        display: true,
    text: 'Top 5 Productos más vendidos'
                },
    scales: {
        yAxes: [{
        ticks: {
        beginAtZero: true
                        }
                    }]
                }
            }
        });
    });
</script>
