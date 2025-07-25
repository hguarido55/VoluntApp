# VoluntApp

# 🤝 VoluntApp — Plataforma de Conexión entre Voluntarios y ONGs

**VoluntApp** es un prototipo de aplicación web desarrollado como proyecto académico con el objetivo de facilitar la conexión entre personas interesadas en realizar voluntariado y organizaciones sin ánimo de lucro (ONGs).

---

## 👨‍💻 Autores

- Hugo Guarido Domínguez  
- José Alejandro Viveros  
- Vittorio Perillo  
- Mario Martínez Lozano  
- Ignacio Pérez de Avilés  

---

## 💻 Tecnologías Utilizadas

- ⚙️ **ASP.NET** — Desarrollo del backend y lógica del servidor  
- 🐬 **MySQL** — Base de datos relacional  
- 🧰 **XAMPP** — Servidor local para pruebas con Apache y MySQL  
- 💻 **C#** — Lenguaje principal para la lógica del servidor  
- 🟪 **Visual Studio** — Entorno de desarrollo

---

## 🚀 Funcionalidades del Prototipo

✔️ **Autenticación de usuarios**: Registro y login de usuarios voluntarios.  
✔️ **Login de administrador**: Acceso especial con funciones de gestión.  
✔️ **Conexión a base de datos local (MySQL)**: Almacenamiento de usuarios, actividades, inscripciones, etc.  
✔️ **Visualización e inscripción a actividades**: Listado de oportunidades con información detallada.  
✔️ **Gestión de actividades por parte del administrador**: Crear, editar y eliminar actividades.  
✔️ **Dashboard personalizado**: Cada usuario puede ver sus actividades y datos personales.  
✔️ **Generador de PDF**: Se genera un documento con el historial del usuario (actividades realizadas, horas, etc.).  
✔️ **Mapa interactivo**: Ubicación de actividades con geolocalización visual.

---

## 🗄️ Base de Datos

El repositorio incluye un script esquema_voluntariado.sql que permite crear fácilmente las tablas necesarias en tu propio servidor MySQL local. Puedes usar XAMPP para iniciar MySQL y luego importar el script desde phpMyAdmin o cualquier cliente de base de datos compatible.

---

## 📘 Memoria del Proyecto

Se incluye un archivo `Memoria_VoluntApp.pdf` en el repositorio que contiene:

- Descripción del contexto y motivación del proyecto  
- Análisis técnico y estructural del sistema  
- Diagramas de arquitectura y base de datos  
- Detalles de implementación y pruebas realizadas
- Instrucciones y manual de uso

📎 **Puedes acceder a la memoria desde la raíz del proyecto**

---

## 📌 Notas Adicionales

- Este proyecto es un prototipo académico y no está optimizado para producción.  
- Se pueden incluir funcionalidades futuras como validación avanzada, autenticación con tokens y despliegue en la nube.  
