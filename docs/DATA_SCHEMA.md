# Data schema notes

Each door record should contain:
- stable `id` and `schemaVersion`
- nominal dimensions and units
- arch parameters
- member list with semantic role and profile code
- normalized source geometry (not only AutoCAD handles)
- anchor/constraint/resize rules

AutoCAD handles are drawing-local identifiers and are not reliable as the sole long-term source geometry reference after copying/importing drawings.

Profile records should store cross-section geometry, material, nominal dimensions, cavities/voids and manufacturing metadata. Sample profile files are schema examples only.
